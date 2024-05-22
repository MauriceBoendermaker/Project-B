using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace MegaBios
{
    public class CreateAccount
    {
        public static void CreateNewAccount(List<TestAccount> jsonData)
        {
            Console.WriteLine("\nCreëer nieuw account");
            Console.WriteLine("--------------------");

            Console.Write("Voer voornaam in: ");
            string voornaam = Console.ReadLine();

            Console.Write("Voer tussenvoegsel in (als u een tussenvoegsel heeft): ");
            string tussenvoegsel = Console.ReadLine();

            Console.Write("Voer achternaam in: ");
            string achternaam = Console.ReadLine();

            Console.Write("Voer geboortedatum in (YYYY-MM-DD): ");
            string geboorteDatum = Console.ReadLine();

            Dictionary<string, string> adres = new Dictionary<string, string>();
            Console.Write("Voer straatnaam in: ");
            adres["straat"] = Console.ReadLine();

            Console.Write("Voer huisnummer in: ");
            adres["huisnummer"] = Console.ReadLine();

            Console.Write("Voer woonplaats in: ");
            adres["woonplaats"] = Console.ReadLine();

            Console.Write("Voer postcode in: ");
            adres["postcode"] = Console.ReadLine();

            string email;
            while (true)
            {
                Console.Write("Voer email in: ");
                email = Console.ReadLine()!;

                if (IsValidEmail(email))
                {
                    break;
                }
                else
                {
                    Console.WriteLine("Ongeldig emailadres. Probeer het opnieuw.");
                }
            }

            string wachtwoord;
            while (true)
            {
                Console.WriteLine("Voer wachtwoord in: ");
                string inputWachtwoord = HelperFunctions.MaskPasswordInput();

                Console.WriteLine("Bevestig wachtwoord: ");
                string confirmWachtwoord = HelperFunctions.MaskPasswordInput();

                if (inputWachtwoord == confirmWachtwoord)
                {
                    wachtwoord = inputWachtwoord;
                    break;
                }
                else
                {
                    Console.WriteLine("Wachtwoorden komen niet overeen. Probeer het opnieuw.");
                }
            }

            Console.Write("Voer telefoonnummer in: ");
            string telefoonNr = Console.ReadLine()!;

            Console.Write("Voer uw voorkeur voor betaalwijze in: ");
            string betaalwijze = Console.ReadLine()!;

            bool is_student;
            while (true)
            {
                Console.Write("Bent u student? (true/false): ");
                string is_studentString = Console.ReadLine()!;

                if (is_studentString == "true" || is_studentString == "false")
                {
                    is_student = Convert.ToBoolean(is_studentString);
                    break;
                }
                else
                {
                    Console.WriteLine("Ongeldige invoer. Voer 'true' of 'false' in.");
                }
            }

            TestAccount newAccount = new TestAccount(voornaam, tussenvoegsel, achternaam, geboorteDatum, adres, email, wachtwoord, telefoonNr, betaalwijze, is_student);
            jsonData.Add(newAccount);

            JsonFunctions.WriteToJson("../../../customers.json", jsonData);

            Console.WriteLine("Succesvol nieuw account gemaakt!");
        }

        private static bool IsValidEmail(string email)
        {
            string pattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
            return Regex.IsMatch(email, pattern);
        }

    }



}
