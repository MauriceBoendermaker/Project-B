namespace MegaBios
{
    public class UpdateAccount
    {
        public static void UpdateField(Account account)
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
                Console.WriteLine("Voor een of andere reden was uw account niet gevonden");
                return;
            }

            List<string> menuOptions = new() {"E-Mail", "Wachtwoord", "Student status"};
            // Console.WriteLine("1. Email2. Wachtwoord\n3. Student status");
            int selectedChoice = MenuFunctions.Menu(menuOptions, null, true);
            
            switch (selectedChoice)
            {
                case -1:
                    return;
                case 0:
                    Console.WriteLine("Voer de nieuwe email in");

                    string newEmail = Console.ReadLine()!;

                    Program.jsonData[index].Email = newEmail;
                    JsonFunctions.WriteToJson("../../../customers.json", Program.jsonData);
                    break;
                case 1:
                    while (true)
                    {
                        Console.WriteLine("Voer het nieuwe wachtwoord in");
                        string newPassword = Console.ReadLine()!;
                        Console.WriteLine("Bevestig het wachtwoord");

                        if (newPassword == Console.ReadLine())
                        {
                            Program.jsonData[index].Wachtwoord = newPassword;
                            JsonFunctions.WriteToJson("../../../customers.json", Program.jsonData);
                            break;
                        }
                        else
                        {
                            Console.WriteLine("Wachtwoorden komen niet overeen!");
                        }
                    }
                    break;
                case 2:
                    Console.WriteLine("Bent u student? (ja/nee)");

                    while (true)
                    {
                        string studentInput = Console.ReadLine()!;

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
                            Console.WriteLine("Ongeldige invoer!");
                        }
                    }
                    break;
            }
            JsonFunctions.WriteToJson("../../../customers.json", Program.jsonData);
            System.Console.WriteLine("Uw gegevens zijn geupdated!. Druk op een willekeurige knop om terug te keren");

            Console.ReadKey(true);

            return;
        }
    }
}
