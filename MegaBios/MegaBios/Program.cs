using System;
using System.IO;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace MegaBios
{
    class Program
    {
        public static List<TestAccount> jsonData = new();
        static void Main(string[] args)
        {

            // JSON bestand ophalen
            string jsonFilePath = "../../../customers.json";
            string jsonText = File.ReadAllText(jsonFilePath);
            JsonDocument jsonDocument = JsonDocument.Parse(jsonText);
            JsonElement root = jsonDocument.RootElement;
            jsonData = JsonFunctions.ConvertJsonToList(root);

            Console.WriteLine("Welcome to MegaBios!");
            Console.WriteLine("Please select an option:");
            Console.WriteLine("1. Create Account");
            Console.WriteLine("2. Login");

            int choice = int.Parse(Console.ReadLine());

            switch (choice)
            {
                case 1:
                    CreateAccount.CreateNewAccount(jsonData);
                    break;
                case 2:
                    Login();
                    break;
                default:
                    Console.WriteLine("Invalid choice. Please try again.");
                    break;
            }

            static void Login()
            {
                Console.WriteLine("Login Form");
                Console.WriteLine("-----------");

                Console.Write("Enter email: ");
                string username = Console.ReadLine();

                Console.Write("Enter password: ");
                string password = HelperFunctions.MaskPasswordInput();

                bool isAuthenticated = false;

                foreach (TestAccount account in jsonData)
                {
                    if (account.Email == username && account.Wachtwoord == password)
                    {
                        isAuthenticated = true;
                        Console.WriteLine("Login successful!");

                        Console.WriteLine("1. Display Account Information \n2. Delete Account\n3. Update Account Information");
                        string userChoice = Console.Readline();

                        switch (userChoice)
                        {
                            case "1":
                                ReadAccount.DisplayUserInfo(account);
                                break;
                            case "2":
                                DeleteAccount.RemoveAccount(jsonData, account);
                                break;
                            case "3":
                                // Hier komt code van John
                                break;
                            default:
                                Console.WriteLine("Invalid choice. Please try again.");
                                break;
                        }
                        break;
                    }
                }

                if (!isAuthenticated)
                {
                    Console.WriteLine("Invalid username or password. Please try again.");
                    // TODO: Code toevoegen voor verkeerde login pogingen
                }
            }

            //CreateAccount.CreateNewAccount(jsonData);
        }
    }
}