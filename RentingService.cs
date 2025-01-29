using System;
using System.Collections.Generic;
using System.Linq;

class RentingService
{
    // En liste over bøker og hvor mange av hver som er tilgjengelige
    private Dictionary<Book, int> bookInventory;
    // En liste over hvor mange av hver bok som er lånt ut
    private Dictionary<Book, int> currentlyBorrowed;
    // En liste over utlånte bøker og deres retur dato
    private Dictionary<Book, BorrowReciept> borrowedBooks;

    public RentingService()
    {
        // Oppretter noen bøker med tittel og forfatter
        Book martian = new Book("Martian", "Jim");
        Book foundation = new Book("Foundation", "Jack");
        Book thewitcher = new Book("The Witcher", "Andrzej Sapkowski");
        Book harrypotter = new Book("Harry Potter", "Rowling");

        // Initialiserer bibliotekets datastruktur
        bookInventory = new Dictionary<Book, int>();
        currentlyBorrowed = new Dictionary<Book, int>();
        borrowedBooks = new Dictionary<Book, BorrowReciept>();

        // Legger bøker til biblioteket med antall tilgjengelig
        bookInventory.Add(martian, 10);
        currentlyBorrowed.Add(martian, 0);
        bookInventory.Add(foundation, 2);
        currentlyBorrowed.Add(foundation, 0);
        bookInventory.Add(thewitcher, 5);
        currentlyBorrowed.Add(thewitcher, 0);
        bookInventory.Add(harrypotter, 10);
        currentlyBorrowed.Add(harrypotter, 2);
    }

    // Returnerer en liste over alle bøker og hvor mange som er tilgjengelige
    public Dictionary<Book, int> ListAllBooks()
    {
        return bookInventory;
    }

    // Lar en bruker låne en bok hvis den er tilgjengelig
    public BorrowReciept? BorrowBook(string title)
    {
        // Sjekker om boken finnes i biblioteket
        var inventoryEntry = bookInventory.FirstOrDefault(entry => entry.Key.Title == title);
        Book book = inventoryEntry.Key;
        int inventoryAmount = inventoryEntry.Value;

        // Hvis boken ikke finnes, returnerer vi null
        if (book == null)
        {
            return null;
        }

        // Sjekker hvor mange eksemplarer av boken som allerede er lånt ut
        int borrowedAmount = currentlyBorrowed[book];
        bool isAvailable = inventoryAmount - borrowedAmount >= 1;

        // Hvis ingen eksemplarer er tilgjengelige, returner null
        if (!isAvailable)
        {
            return null;
        }

        // Lager en lånekvittering
        BorrowReciept reciept = new BorrowReciept(book.Title);
        // Øker antallet utlånte eksemplarer
        currentlyBorrowed[book]++;
        // Lagrer kvitteringen for utlånte bøker
        borrowedBooks.Add(book, reciept);
        return reciept;
    }

    // Lar en bruker returnere en bok
    public ReturnReciept? ReturnBook(string title)
    {
        // Finner boken i biblioteket basert på tittel
        var bookEntry = bookInventory.Keys.FirstOrDefault(book => book.Title == title);

        // Sjekker om boken finnes og er utlånt
        if (bookEntry == null || currentlyBorrowed[bookEntry] == 0)
        {
            return null; 
        }

        // Henter lånekvitteringen for boken
        BorrowReciept borrowReciept = borrowedBooks[bookEntry];
        DateTime dueDate = borrowReciept.DueDate;

        // Sjekker om boken returneres i tide
        bool isReturnedOnTime = DateTime.Today <= dueDate;
        string returnStatus = isReturnedOnTime ? "on time" : "late";

        // Reduserer antall utlånte eksemplarer
        currentlyBorrowed[bookEntry]--;
        // Fjerner boken fra lånte bøker
        borrowedBooks.Remove(bookEntry);
        
        // Returnerer en returkvittering
        return new ReturnReciept(bookEntry.Title, returnStatus, dueDate);
    }
}

// Representerer en bok med tittel og forfatter
class Book
{
    public string Title; // Boktittel
    public string Author; // Forfatter

    public Book(string title, string author)
    {
        Title = title;
        Author = author;
    }
}

// Kvittering for lån av en bok
class BorrowReciept
{
    public DateTime BorrowingDate; // Dato for lån
    public DateTime DueDate; // Forfallsdato
    public string BookTitle; // Tittel på boken som lånes

    public BorrowReciept(string bookTitle)
    {
        BookTitle = bookTitle;
        BorrowingDate = DateTime.Today; // Setter lånedato til i dag
        DueDate = DateTime.Today.AddDays(30); // Lånetid er 30 dager
    }
}

// Kvittering for retur av en bok
class ReturnReciept
{
    public string BookTitle { get; } // Tittel på boken som returneres
    public DateTime ReturnDate { get; } // Dato for retur
    public string ReturnStatus { get; } // Status (on time or late)
    public DateTime DueDate { get; } // Forfallsdato

    public ReturnReciept(string bookTitle, string returnStatus, DateTime dueDate)
    {
        BookTitle = bookTitle;
        ReturnDate = DateTime.Today; // Setter retur dato til dagens dato
        ReturnStatus = returnStatus;
        DueDate = dueDate;
    }
}


