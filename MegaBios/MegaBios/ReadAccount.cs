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
            Console.WriteLine($"Straat: {loggedInUser.Adres["straat"]} {loggedInUser.Adres["huisnummer"]}");
            Console.WriteLine($"Postcode: {loggedInUser.Adres["postcode"]} {loggedInUser.Adres["woonplaats"]}");
            Console.WriteLine($"Email: {loggedInUser.Email}");
            Console.WriteLine($"Telefoonnummer: {loggedInUser.TelefoonNr}");
            Console.WriteLine($"Voorkeur betaalwijze: {loggedInUser.Voorkeur_Betaalwijze}");
            Console.WriteLine($"Student: {(loggedInUser.IsStudent ? "ja" : "nee")}");
        }
    }
}
