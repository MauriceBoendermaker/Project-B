using System.Text;

namespace MegaBios
{
    public static class MenuFunctions
    {
        // Menu maar met optioneel bericht
        public static int Menu<T>(List<T> menuOptions, StringBuilder optionalMessage = null, bool canGoBack = true)
        {
            int cursorPos = 0;

            while (true)
            {
                if (Environment.GetEnvironmentVariable("IS_TEST_ENVIRONMENT") != "true")
                {
                    Console.Clear();
                }
                if (optionalMessage != null) {
                    Console.WriteLine(optionalMessage);
                }
                
                Console.WriteLine("Selecteer een optie met de pijltjestoetsen. Druk op 'Enter' om je keuze te bevestigen.");

                if (canGoBack)
                {
                    Console.WriteLine("Druk op 'Backspace' om terug te gaan");
                }

                Console.WriteLine("");
                StringBuilder menuText = new StringBuilder();

                for (int i = 0; i < menuOptions.Count; i++)
                {
                    if (cursorPos == i)
                    {
                        menuText.AppendLine($"\x1b[42m{menuOptions[i]}\x1b[0m");
                    }
                    else
                    {
                        menuText.AppendLine($"{menuOptions[i]}");
                    }
                }

                Console.WriteLine(menuText);
                ConsoleKeyInfo keyInfo = Console.ReadKey(true);

                if (keyInfo.Key == ConsoleKey.Enter)
                {
                    return cursorPos;
                }
                else if (keyInfo.Key == ConsoleKey.Backspace && canGoBack)
                {
                    Console.WriteLine("Weet je zeker dat je terug wilt gaan? Druk dan op 'Backspace'");

                    if (Console.ReadKey(true).Key == ConsoleKey.Backspace)
                    {
                        return -1;
                    }
                }
                else
                {
                    cursorPos = MenuFunctions.MoveCursor(cursorPos, keyInfo, menuOptions.Count);
                }
            }
        }

        // Menu maar voor DateTime-functionaliteit
        public static int Menu(List<DateTime> menuOptions, bool showTimes, bool canGoBack = true)
        {
            int cursorPos = 0;

            while (true)
            {
                Console.Clear();
                Console.WriteLine("Selecteer een optie met de pijltjestoetsen. Druk op 'Enter' om je keuze te bevestigen");

                if (canGoBack)
                {
                    Console.WriteLine("Druk op 'Backspace' om terug te gaan");
                }

                Console.WriteLine("");
                StringBuilder menuText = new StringBuilder();

                for (int i = 0; i < menuOptions.Count; i++)
                {
                    if (showTimes)
                    {
                        string dateTimeString = menuOptions[i].ToString("dd/MM/yyyy Hh:mm:ss");
                        if (cursorPos == i)
                        {
                            
                            menuText.AppendLine($"\x1b[42m{dateTimeString}\x1b[0m");
                        }
                        else
                        {
                            menuText.AppendLine($"{dateTimeString}");
                        }
                    }
                    else
                    {
                        string dateString = menuOptions[i].ToString("dd/MM/yyyy");
                        if (cursorPos == i)
                        {
                            menuText.AppendLine($"\x1b[42m{dateString}\x1b[0m");
                        }
                        else
                        {
                            menuText.AppendLine($"{dateString}");
                        }
                    }
                }

                Console.WriteLine(menuText);
                ConsoleKeyInfo keyInfo = Console.ReadKey(true);

                if (keyInfo.Key == ConsoleKey.Enter)
                {
                    return cursorPos;
                }
                else if (keyInfo.Key == ConsoleKey.Backspace && canGoBack)
                {
                    Console.WriteLine("Weet je zeker dat je terug wilt gaan? Druk dan op 'Backspace'");

                    if (Console.ReadKey(true).Key == ConsoleKey.Backspace)
                    {
                        return -1;
                    }
                }
                else
                {
                    cursorPos = MenuFunctions.MoveCursor(cursorPos, keyInfo, menuOptions.Count);
                }
            }
        }

        public static int MoveCursor(int currentPos, ConsoleKeyInfo pressedKeyInfo, int maxRange)
        {
            if (pressedKeyInfo.Key == ConsoleKey.UpArrow)
            {
                if (currentPos <= 0)
                {
                    currentPos = maxRange - 1;
                }
                else
                {
                    currentPos--;
                }
            }
            else if (pressedKeyInfo.Key == ConsoleKey.DownArrow)
            {
                if (currentPos >= maxRange - 1)
                {
                    currentPos = 0;
                }
                else
                {
                    currentPos++;
                }
            }

            return currentPos;
        }
    }
}
