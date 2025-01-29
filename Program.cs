// Starter Renting Service-programmet
Console.WriteLine("Starting Renting Service");

// Oppretter en instans av RentingService for å håndtere utlån og retur av bøker
RentingService rentingService = new RentingService();

while (true) // Uendelig løkke for å holde programmet kjørende
{
  // Leser inn brukerens input fra konsollen
  string? input = Console.ReadLine();
  
  // Hvis input er null (f.eks. ved uventet avslutning), avslutt programmet
  if (input == null)
  {
    Environment.Exit(1);
  }

  // Sjekker hvilken kommando brukeren skrev inn
  switch (input)
  {
    case "list":
      // Henter en liste over tilgjengelige bøker og lagrer den i en ordbok
      Dictionary<Book, int> availableBooks = rentingService.ListAllBooks();

      // Går gjennom listen over tilgjengelige bøker og skriver ut titlene
      foreach (var bookEntry in availableBooks)
      {
        Console.WriteLine(bookEntry.Key.Title);
      }
      break;

    case "borrow":
      // Starter prosessen for å låne en bok
      Console.WriteLine("Borrowing book");

      // Leser inn tittelen på boken som brukeren ønsker å låne
      string? bookTitleInput = Console.ReadLine();

      // Hvis input er null, avslutt programmet
      if (bookTitleInput == null)
      {
        Environment.Exit(1);
      }

      // Forsøker å låne boken og mottar en kvittering (hvis suksess)
      BorrowReciept? reciept = rentingService.BorrowBook(bookTitleInput);

      // Sjekker om boken ble funnet og lånt ut
      if (reciept == null)
      {
        Console.WriteLine($"No book with title {bookTitleInput} found");
      }
      else
      {
        // Viser suksessmelding og returfrist
        Console.WriteLine($"Congratulation! Book borrowed. Please return it by: {reciept.DueDate}");
      }
      break;

    case "return":
      // Placeholder for bokretur-funksjonalitet (ikke implementert ennå)
      Console.WriteLine("Returning book");
      break;

    default:
      // Gir beskjed om at brukeren skrev inn en ugyldig kommando
      Console.WriteLine("Invalid Input");
      break;
  }
}


