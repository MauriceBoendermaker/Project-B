using System;

namespace MegaBios
{
    public static class ReadAccount
    {
        public static void DisplayUserInfo(TestAccount loggedInUser)
        {
            Console.WriteLine("User Information:");
            Console.WriteLine("-----------------");
            Console.WriteLine($"Name: {loggedInUser.Voornaam} {loggedInUser.Tussenvoegsel} {loggedInUser.Achternaam}");
            Console.WriteLine($"Date of Birth: {loggedInUser.GeboorteDatum}");
            Console.WriteLine("Address:");
            Console.WriteLine($"{loggedInUser.Adres}");
            // Console.WriteLine($"  {loggedInUser.Adres.straat});
            // Console.WriteLine($"  {loggedInUser.Adres.Postcode} {loggedInUser.Adres.Woonplaats}");
            Console.WriteLine($"Email: {loggedInUser.Email}");
            Console.WriteLine($"Phone Number: {loggedInUser.TelefoonNr}");
            Console.WriteLine($"Preferred Payment Method: {loggedInUser.Voorkeur_Betaalwijze}");
            Console.WriteLine($"Student: {(loggedInUser.IsStudent ? true : false)}");
        }
    }
}