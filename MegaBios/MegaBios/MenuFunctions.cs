using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace MegaBios
{
    public static class MenuFunctions {
        public static int MoveCursor(int currentPos, ConsoleKeyInfo pressedKeyInfo, int maxRange) {
            if (pressedKeyInfo.Key == ConsoleKey.UpArrow) {
                if (currentPos == 0) {
                    currentPos = maxRange;
                }
                else {
                    currentPos--;
                }
            }
            else if (pressedKeyInfo.Key == ConsoleKey.DownArrow) {
                if ((currentPos == maxRange)) {
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