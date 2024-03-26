using System.Collections;
using System.Net.Sockets;

namespace MegaBios
{
    public class UpdateAccount
    {

        public static void UpdateField(TestAccount account) {
            int index = -1;
            for (int i = 0; i < Program.jsonData.Count; i++) {
                if (Program.jsonData[i] == account) {
                    index = i;
                }
            }
            if (index == -1) {
                System.Console.WriteLine("For some reason. your account was not found...");
                return;
            }
            Console.WriteLine("What would you like to update? (First name (1), Middle name (2), Last name (3), Birth Date (4), Address (5),\nEmail (6), Password(7), Phone number (8), Payment method (9), Student status (10))");
            Console.WriteLine("Enter only the number of the option you want to update!");
            Console.WriteLine("Enter your choice: ");
            bool loopBreak = false;            
            while (true) {
                switch (Console.ReadLine()) {
                    case "1":
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
                        Dictionary<string, string> newAdres = new() {{"straat", newStraatnaam}, {"huisnummer", newHouseNumber}, {"woonplaats", newWoonPlaats} , {"postcode", newPostCode}};
                        Program.jsonData[index].Adres = newAdres;
                        JsonFunctions.WriteToJson(Program.jsonFilePath, Program.jsonData);
                        loopBreak = true;
                        break;
                    case "6":
                        System.Console.WriteLine("Please enter your email");
                        string newEmail = Console.ReadLine()!;
                        Program.jsonData[index].Email = newEmail;
                        JsonFunctions.WriteToJson(Program.jsonFilePath, Program.jsonData);
                        loopBreak = true;
                        break;
                    case "7":
                        while(true) {
                            System.Console.WriteLine("Please enter your password");
                            string newPassword = Console.ReadLine()!;
                            System.Console.WriteLine("Please confirm your password");
                            if (newPassword == Console.ReadLine()) {
                                Program.jsonData[index].Wachtwoord = newPassword;
                                JsonFunctions.WriteToJson(Program.jsonFilePath, Program.jsonData);
                                loopBreak = true;
                                break;
                            }
                            else {
                                System.Console.WriteLine("The two passwords are different!");
                            }
                        }
                        break;
                    case "8":
                        System.Console.WriteLine("enter your phone number please (Format +XX XX XXXXXXXX)");
                        string newPhonenumber = Console.ReadLine()!;
                        Program.jsonData[index].TelefoonNr = newPhonenumber;
                        JsonFunctions.WriteToJson(Program.jsonFilePath, Program.jsonData);
                        loopBreak = true;
                        break;
                    case "9":
                        System.Console.WriteLine("Please enter your preferred method of payment (Creditcard, iDeal, PayPal)");
                        while (true) {
                            string newPaymentMethod = Console.ReadLine()!;
                            if (new ArrayList{"Creditcard", "iDeal", "PayPal"}.Contains(newPaymentMethod)) {
                                Program.jsonData[index].Voorkeur_Betaalwijze = newPaymentMethod;
                                JsonFunctions.WriteToJson(Program.jsonFilePath, Program.jsonData);
                                loopBreak = true;
                                break;
                            }
                            else {
                                System.Console.WriteLine("Not a valid payment method!");
                            }
                        }
                        break;
                    case "10":
                        System.Console.WriteLine("Are you a student? (yes/no)");
                        while (true) {
                            if (System.Console.ReadLine() == "yes") {
                                Program.jsonData[index].IsStudent = true;
                                break;
                            }
                            else if (System.Console.ReadLine() == "no") {
                                Program.jsonData[index].IsStudent = false;
                                break;
                            }
                            else {
                                System.Console.WriteLine("Invalid input!");
                            }
                        }
                        JsonFunctions.WriteToJson(Program.jsonFilePath, Program.jsonData);
                        loopBreak = true;
                        break;
                }
                if (loopBreak) {
                    break;
                }
            }
        }
    }
}