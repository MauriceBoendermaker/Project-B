namespace MegaBios
{
    public class ReadAccount : IReadAccount
    {
        public void DisplayUserInfo(Account loggedInUser)
        {
            Console.WriteLine("Gebruikersinformatie:");
            Console.WriteLine("-----------------");
            Console.WriteLine($"Naam: {loggedInUser.Voornaam} {loggedInUser.Tussenvoegsel} {loggedInUser.Achternaam}");
            Console.WriteLine($"Geboortedatum : {loggedInUser.GeboorteDatum}");
            Console.WriteLine("Adres:");
            Console.WriteLine($"Adres: {loggedInUser.Adres["straat"]} {loggedInUser.Adres["huisnummer"]}");
            Console.WriteLine($"{loggedInUser.Adres["postcode"]} {loggedInUser.Adres["woonplaats"]}");
            Console.WriteLine($"Email: {loggedInUser.Email}");
            Console.WriteLine($"Telefoonnummer: {loggedInUser.TelefoonNr}");
            Console.WriteLine($"Voorkeur betaalwijze: {loggedInUser.Voorkeur_Betaalwijze}");
            Console.WriteLine($"Student: {(loggedInUser.IsStudent ? true : false)}");
        }
    }
}
