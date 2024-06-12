using System.Text.RegularExpressions;

namespace MegaBios
{
    public static class CreateAccount
    {
        public static void CreateNewAccount(List<Account> jsonData)
        {
            Console.WriteLine("\nCreëer nieuw account");
            System.Console.WriteLine("Druk op enter om je input te bevestigen");
            Console.WriteLine("--------------------");

            string voornaam;
            while (true)
            {
                Console.Write("Voer voornaam in: ");
                voornaam = Console.ReadLine()!;
                if (Regex.IsMatch(voornaam, @"^[a-zA-Z]+$"))
                {
                    break;
                }
                else
                {
                    Console.WriteLine("Ongeldige invoer. Voer alleen letters in.");
                }
            }

            string tussenvoegsel;
            while (true)
            {
                Console.Write("Voer tussenvoegsel in (als u een tussenvoegsel heeft): ");
                tussenvoegsel = Console.ReadLine()!;
                if (string.IsNullOrEmpty(tussenvoegsel) || Regex.IsMatch(tussenvoegsel, @"^[a-zA-Z]+$"))
                {
                    break;
                }
                else
                {
                    Console.WriteLine("Ongeldige invoer. Voer alleen letters in.");
                }
            }

            string achternaam;
            while (true)
            {
                Console.Write("Voer achternaam in: ");
                achternaam = Console.ReadLine()!;
                if (Regex.IsMatch(achternaam, @"^[a-zA-Z]+$"))
                {
                    break;
                }
                else
                {
                    Console.WriteLine("Ongeldige invoer. Voer alleen letters in.");
                }
            }

            string geboorteDatum;
            while (true)
            {
                Console.Write("Voer geboortedatum in (DD-MM-YYYY): ");
                geboorteDatum = Console.ReadLine()!;
                if (DateTime.TryParseExact(geboorteDatum, "dd-MM-yyyy", null, System.Globalization.DateTimeStyles.None, out _))
                {
                    break;
                }
                else
                {
                    Console.WriteLine("Ongeldige invoer. Voer een geldige datum in het formaat YYYY-MM-DD in.");
                }
            }
            Dictionary<string, string> adres = new Dictionary<string, string>();
            while (true)
            {
                Console.Write("Wilt u nieuwsbrieven ontvangen?\nHiervoor moet u uw adres invullen (Ja/Nee): ");
                string wantLetters = Console.ReadLine()?.ToLower(); // Convert input naar lowercase
                
                if (wantLetters == "ja")
                {
                    Console.Write("Voer straatnaam in: ");
                    adres["straat"] = Console.ReadLine()!;

                    Console.Write("Voer huisnummer in: ");
                    adres["huisnummer"] = Console.ReadLine()!;

                    Console.Write("Voer woonplaats in: ");
                    adres["woonplaats"] = Console.ReadLine()!;

                    Console.Write("Voer postcode in: ");
                    adres["postcode"] = Console.ReadLine()!;

                    break;
                }
                else if (wantLetters == "nee") {
                    adres["straat"] = "";
                    adres["huisnummer"] = "";
                    adres["woonplaats"] = "";
                    adres["postcode"] = "";
                    break;
                }
                else 
                {
                    Console.WriteLine("Ongeldige invoer. Voer 'Ja' of 'Nee' in.");
                }
            }    
            
            string email;
            while (true)
            {
                Console.Write("Voer email in: ");
                email = Console.ReadLine()!;

                if (IsValidEmail(email))
                {
                    Console.WriteLine("Email is geldig.");
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
                    Console.WriteLine("Wachtwoorden komen overeen.");
                    break;
                }
                else
                {
                    Console.WriteLine("Wachtwoorden komen niet overeen. Probeer het opnieuw.");
                }
            }

            Console.Write("Voer telefoonnummer in: ");
            string telefoonNr = Console.ReadLine()!;

            bool is_student;
            while (true)
            {
                Console.Write("Bent u student? (Ja/Nee): ");
                string is_studentString = Console.ReadLine()?.ToLower(); // Convert input naar lowercase

                if (is_studentString == "ja" || is_studentString == "nee")
                {
                    // Convert "Ja" to true, "Nee" to false
                    is_student = (is_studentString == "ja");

                    Console.WriteLine("Geldige student status ingevoerd.");

                    break;
                }
                else
                {
                    Console.WriteLine("Ongeldige invoer. Voer 'Ja' of 'Nee' in.");
                }
            }


            Account newAccount = new Account(voornaam, tussenvoegsel, achternaam, geboorteDatum, adres, email, wachtwoord, telefoonNr, is_student, new List<Reservation>(), new List<Reservation>());
            jsonData.Add(newAccount);
            Console.WriteLine("Nieuw account toegevoegd aan de lijst.");

            JsonFunctions.WriteToJson("../../../customers.json", jsonData);

            Console.WriteLine("Account opgeslagen in JSON bestand.");
            Console.WriteLine("Succesvol nieuw account gemaakt!");
        }

        public static bool IsValidEmail(string email)
        {
            string pattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
            return Regex.IsMatch(email, pattern);
        }
    }
}
