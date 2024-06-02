using System.Text;

namespace MegaBios
{
    public static class MenuFunctions {
        // Menu but with optional message
        public static int Menu<T>(List<T> menuOptions, StringBuilder optionalMessage, bool canGoBack = true) {
            int cursorPos = 0;
            while(true) {

                Console.Clear();
                System.Console.WriteLine(optionalMessage);
                Console.WriteLine("Selecteer een optie met de pijltjestoetsen. Druk op Enter om je keuze te bevestigen.");
                if (canGoBack) {
                    System.Console.WriteLine("Druk op Backspace om terug te gaan");

                }
                System.Console.WriteLine("");

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
                System.Console.WriteLine(menuText);

                ConsoleKeyInfo keyInfo = Console.ReadKey(true);

                if (keyInfo.Key == ConsoleKey.Enter && canGoBack)
                {
                    return cursorPos;
                }
                else if (keyInfo.Key == ConsoleKey.Backspace) {
                    System.Console.WriteLine("Weet je zeker dat je terug wilt gaan? Druk dan op Backspace");
                    if (Console.ReadKey(true).Key == ConsoleKey.Backspace) {

                        return -1;
                    }
                }
                else
                {
                    cursorPos = MenuFunctions.MoveCursor(cursorPos, keyInfo, menuOptions.Count);
                }
            }
        }
        // Menu but for DateTime functionality
        public static int Menu(List<DateTime> menuOptions, bool showTimes, bool canGoBack = true) {

            int cursorPos = 0;
            while(true) {

                Console.Clear();
                Console.WriteLine("Selecteer een optie met de pijltjestoetsen. Druk op Enter om je keuze te bevestigen");
                if (canGoBack) {
                    System.Console.WriteLine("Druk op Backspace om terug te gaan");

                }
                System.Console.WriteLine("");

                StringBuilder menuText = new StringBuilder();

                for (int i = 0; i < menuOptions.Count; i++)
                {
                    if (showTimes) {
                        if (cursorPos == i)
                        {
                            menuText.AppendLine($"\x1b[42m{menuOptions[i]}\x1b[0m");
                        }
                        else
                        {
                            menuText.AppendLine($"{menuOptions[i]}");
                        }
                    }
                    else {
                        if (cursorPos == i)
                        {
                            menuText.AppendLine($"\x1b[42m{menuOptions[i].ToString("yyyy/MM/dd")}\x1b[0m");
                        }
                        else
                        {   
                            menuText.AppendLine($"{menuOptions[i].ToString("yyyy/MM/dd")}");
                        }
                    }
                    
                }
                System.Console.WriteLine(menuText);

                ConsoleKeyInfo keyInfo = Console.ReadKey(true);

                if (keyInfo.Key == ConsoleKey.Enter)
                {
                    return cursorPos;
                }
                else if (keyInfo.Key == ConsoleKey.Backspace && canGoBack) {
                    System.Console.WriteLine("Weet je zeker dat je terug wilt gaan? Druk dan op Backspace");
                    if (Console.ReadKey(true).Key == ConsoleKey.Backspace) {

                        return -1;
                    }
                }
                else
                {
                    cursorPos = MenuFunctions.MoveCursor(cursorPos, keyInfo, menuOptions.Count);
                }
            }
        }

        // Regular menu
        public static int Menu<T>(List<T> menuOptions, bool canGoBack = true) {
            int cursorPos = 0;
            while(true) {

                Console.Clear();
                Console.WriteLine("Selecteer een optie met de pijltjestoetsen. Druk op Enter om je keuze te bevestigen");
                if (canGoBack) {
                    System.Console.WriteLine("Druk op Backspace om terug te gaan");

                }
                System.Console.WriteLine("");

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
                System.Console.WriteLine(menuText);

                ConsoleKeyInfo keyInfo = Console.ReadKey(true);

                if (keyInfo.Key == ConsoleKey.Enter)
                {
                    return cursorPos;
                }
                else if (keyInfo.Key == ConsoleKey.Backspace && canGoBack) {
                    System.Console.WriteLine("Weet je zeker dat je terug wilt gaan? Druk dan op Backspace");
                    if (Console.ReadKey(true).Key == ConsoleKey.Backspace) {

                        cursorPos = -1;
                        return cursorPos;
                    }
                }
                else
                {
                    cursorPos = MenuFunctions.MoveCursor(cursorPos, keyInfo, menuOptions.Count);
                }
            }
        }
        public static int MoveCursor(int currentPos, ConsoleKeyInfo pressedKeyInfo, int maxRange) {
            if (pressedKeyInfo.Key == ConsoleKey.UpArrow) {
                if (currentPos <= 0) {

                    currentPos = maxRange - 1;
                }
                else {
                    currentPos--;
                }
            }
            else if (pressedKeyInfo.Key == ConsoleKey.DownArrow) {
                if (currentPos >= maxRange - 1) {
                    currentPos = 0;
                }
                else { 
                    currentPos++;
                }
            }

            return currentPos;
        }
    }
}