using System;

namespace MegaBios
{
    public class CreateAccount
    {
        public static void CreateNewAccount(List<TestAccount> jsonData)
        {
            Console.WriteLine("\nCreate a new account");
            Console.WriteLine("--------------------");

            Console.Write("Enter first name: ");
            string voornaam = Console.ReadLine();

            Console.Write("Enter middle name (if any): ");
            string tussenvoegsel = Console.ReadLine();

            Console.Write("Enter last name: ");
            string achternaam = Console.ReadLine();

            Console.Write("Enter date of birth (YYYY-MM-DD): ");
            string geboorteDatum = Console.ReadLine();

            Dictionary<string, string> adres = new Dictionary<string, string>();
            Console.Write("Enter street: ");
            adres["straat"] = Console.ReadLine();

            Console.Write("Enter house number: ");
            adres["huisnummer"] = Console.ReadLine();

            Console.Write("Enter city: ");
            adres["woonplaats"] = Console.ReadLine();

            Console.Write("Enter postal code: ");
            adres["postcode"] = Console.ReadLine();

            Console.Write("Enter email: ");
            string email = Console.ReadLine();

            Console.Write("Enter password: ");
            string wachtwoord = HelperFunctions.MaskPasswordInput();

            Console.Write("Enter phone number: ");
            string telefoonNr = Console.ReadLine();

            Console.Write("Enter preferred payment method: ");
            string betaalwijze = Console.ReadLine();

            Console.Write("Are you a student? (true/false): ");
            bool is_student = Convert.ToBoolean(Console.ReadLine());

            TestAccount newAccount = new TestAccount(voornaam, tussenvoegsel, achternaam, geboorteDatum, adres, email, wachtwoord, telefoonNr, betaalwijze, is_student);
            jsonData.Add(newAccount);
            JsonFunctions.WriteToJson("../../../customers.json", jsonData);

            Console.WriteLine("New account created successfully!");
        }
    }
}