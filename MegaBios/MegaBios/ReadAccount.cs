using System;

namespace MegaBios
{
    public static class ReadAccount
    {
        public static void DisplayUserInfo(TestAccount loggedInUser)
        {
            Console.WriteLine("Gebruikersinformatie:");
            Console.WriteLine("-----------------");
            Console.WriteLine($"Naam: {loggedInUser.Voornaam} {loggedInUser.Tussenvoegsel} {loggedInUser.Achternaam}");
            Console.WriteLine($"Geboortedatum : {loggedInUser.GeboorteDatum}");
            Console.WriteLine("Adres:");
            Console.WriteLine($"{loggedInUser.Adres}");
            // Console.WriteLine($"  {loggedInUser.Adres.straat});
            // Console.WriteLine($"  {loggedInUser.Adres.Postcode} {loggedInUser.Adres.Woonplaats}");
            Console.WriteLine($"Email: {loggedInUser.Email}");
            Console.WriteLine($"Telefoonnumber: {loggedInUser.TelefoonNr}");
            Console.WriteLine($"Voorkeur betaalwijze: {loggedInUser.Voorkeur_Betaalwijze}");
            Console.WriteLine($"Student: {(loggedInUser.IsStudent ? true : false)}");
        }
    }
}