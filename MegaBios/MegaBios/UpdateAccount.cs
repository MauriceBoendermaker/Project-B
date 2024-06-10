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

            Console.WriteLine("Wat wilt u updaten?\n1. Email\n2. Wachtwoord\n3. Student status");
            Console.WriteLine("Voer alleen het nummer van de optie in die u wilt updaten!");
            Console.WriteLine("Voer je keuze in: ");

            bool loopBreak = false;

            while (true)
            {
                switch (Console.ReadLine())
                {
                    case "1":
                        Console.WriteLine("Voer de nieuwe email in");

                        string newEmail = Console.ReadLine()!;

                        Program.jsonData[index].Email = newEmail;
                        JsonFunctions.WriteToJson("../../../customers.json", Program.jsonData);

                        loopBreak = true;
                        break;
                    case "2":
                        while (true)
                        {
                            Console.WriteLine("Voer het nieuwe wachtwoord in");
                            string newPassword = Console.ReadLine()!;
                            Console.WriteLine("Bevestig het wachtwoord");

                            if (newPassword == Console.ReadLine())
                            {
                                Program.jsonData[index].Wachtwoord = newPassword;
                                JsonFunctions.WriteToJson("../../../customers.json", Program.jsonData);

                                loopBreak = true;
                                break;
                            }
                            else
                            {
                                Console.WriteLine("Wachtwoorden komen niet overeen!");
                            }
                        }
                        break;
                    case "3":
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

                        JsonFunctions.WriteToJson("../../../customers.json", Program.jsonData);

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
