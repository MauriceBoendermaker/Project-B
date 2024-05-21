using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace MegaBios
{
    public static class MenuFunctions {
        
        public static int Menu<T>(List<T> menuOptions, string optionalMessage) {
            int cursorPos = 0;
            while(true) {
                Console.Clear();
                System.Console.WriteLine(optionalMessage);
                Console.WriteLine("Selecteer een optie met de pijltjestoetsen. Druk op Enter om je keuze te bevestigen\n");
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
                else
                {
                    cursorPos = MenuFunctions.MoveCursor(cursorPos, keyInfo, menuOptions.Count);
                }
            }
        }
        public static int Menu<T>(List<T> menuOptions) {
            int cursorPos = 0;
            while(true) {
                Console.Clear();
                Console.WriteLine("Selecteer een optie met de pijltjestoetsen. Druk op Enter om je keuze te bevestigen\n");
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