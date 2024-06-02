namespace MegaBios
{
    public class UpdateAccount
    {
        public static void UpdateField(TestAccount account)
        {
            int index = -1;
            for (int i = 0; i < Program.jsonData.Count; i++)
            {
                if (Program.jsonData[i] == account)
                {
                    index = i;
                }
            }

            if (index == -1)
            {
                System.Console.WriteLine("Voor een of andere reden was uw account niet gevonden");
                return;
            }

            Console.WriteLine("Wat wilt u updaten?\n1. Email\n2. Wachtwoord\n3. Student status");
            Console.WriteLine("Voer alleen het nummer van de optie in die u wilt updaten!");
            Console.WriteLine("Voer je keuze in: ");

            bool loopBreak = false;
            while (true)
            {
                switch (Console.ReadLine())
                {
                    /* case "1":
                         System.Console.WriteLine("Please enter your first name");
                         string newVoornaam = Console.ReadLine()!;
                         Program.jsonData[index].Voornaam = newVoornaam;
                         JsonFunctions.WriteToJson(Program.jsonFilePath, Program.jsonData);
                         loopBreak = true;
                         break;
                     case "2":
                         System.Console.WriteLine("Please enter your middle name");
                         string newTussenvoegsel = Console.ReadLine()!;
                         Program.jsonData[index].Tussenvoegsel = newTussenvoegsel;
                         JsonFunctions.WriteToJson(Program.jsonFilePath, Program.jsonData);
                         loopBreak = true;
                         break;
                     case "3":
                         System.Console.WriteLine("Please enter your last name");
                         string newAchternaam = Console.ReadLine()!;
                         Program.jsonData[index].Achternaam = newAchternaam;
                         JsonFunctions.WriteToJson(Program.jsonFilePath, Program.jsonData);
                         loopBreak = true;
                         break;
                     case "4":
                         System.Console.WriteLine("Please enter your birthdate (yyyy-MM-dd)");
                         string newGeboorteDatum = Console.ReadLine()!;
                         Program.jsonData[index].GeboorteDatum = newGeboorteDatum;
                         JsonFunctions.WriteToJson(Program.jsonFilePath, Program.jsonData);
                         loopBreak = true;
                         break;
                     case "5":
                         System.Console.WriteLine("Please enter your street's name");
                         string newStraatnaam = Console.ReadLine()!;
                         System.Console.WriteLine("Please enter your house number");
                         string newHouseNumber = Console.ReadLine()!;
                         System.Console.WriteLine("Please enter your city/village of residence");
                         string newWoonPlaats = Console.ReadLine()!;
                         System.Console.WriteLine("Please enter your zip-code");
                         string newPostCode = Console.ReadLine()!;
                         Dictionary<string, string> newAdres = new() { { "straat", newStraatnaam }, { "huisnummer", newHouseNumber }, { "woonplaats", newWoonPlaats }, { "postcode", newPostCode } };
                         Program.jsonData[index].Adres = newAdres;
                         JsonFunctions.WriteToJson(Program.jsonFilePath, Program.jsonData);
                         loopBreak = true;
                         break; */
                    case "1":
                        System.Console.WriteLine("Voer de nieuwe email in");
                        string newEmail = Console.ReadLine()!;
                        Program.jsonData[index].Email = newEmail;
                        JsonFunctions.WriteToJson(Program.jsonFilePath, Program.jsonData);
                        loopBreak = true;
                        break;
                    case "2":
                        while (true)
                        {
                            System.Console.WriteLine("Voer het nieuwe wachtwoord in");
                            string newPassword = Console.ReadLine()!;
                            System.Console.WriteLine("Bevestig het wachtwoord");

                            if (newPassword == Console.ReadLine())
                            {
                                Program.jsonData[index].Wachtwoord = newPassword;
                                JsonFunctions.WriteToJson(Program.jsonFilePath, Program.jsonData);
                                loopBreak = true;
                                break;
                            }
                            else
                            {
                                System.Console.WriteLine("Wachtwoorden komen niet overeen!");
                            }
                        }
                        break;
                    /* case "8":
                         System.Console.WriteLine("enter your phone number please (Format +XX XX XXXXXXXX)");
                         string newPhonenumber = Console.ReadLine()!;
                         Program.jsonData[index].TelefoonNr = newPhonenumber;
                         JsonFunctions.WriteToJson(Program.jsonFilePath, Program.jsonData);
                         loopBreak = true;
                         break;
                     case "9":
                         System.Console.WriteLine("Please enter your preferred method of payment (Creditcard, iDeal, PayPal)");
                         while (true)
                         {
                             string newPaymentMethod = Console.ReadLine()!;
                             if (new ArrayList { "Creditcard", "iDeal", "PayPal" }.Contains(newPaymentMethod))
                             {
                                 Program.jsonData[index].Voorkeur_Betaalwijze = newPaymentMethod;
                                 JsonFunctions.WriteToJson(Program.jsonFilePath, Program.jsonData);
                                 loopBreak = true;
                                 break;
                             }
                             else
                             {
                                 System.Console.WriteLine("Not a valid payment method!");
                             }
                         }
                         break; */
                    case "3":
                        System.Console.WriteLine("Bent u student? (ja/nee)");

                        while (true)
                        {
                            string studentInput = System.Console.ReadLine()!;

                            if (studentInput == "ja")
                            {
                                Program.jsonData[index].IsStudent = true;
                                break;
                            }
                            else if (studentInput == "nee")
                            {
                                Program.jsonData[index].IsStudent = false;
                                break;
                            }
                            else
                            {
                                System.Console.WriteLine("Invalide input!");
                            }
                        }
                        JsonFunctions.WriteToJson(Program.jsonFilePath, Program.jsonData);
                        loopBreak = true;
                        break;
                }

                if (loopBreak)
                {
                    break;
                }
            }
        }
    }
}