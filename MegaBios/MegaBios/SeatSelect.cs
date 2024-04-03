namespace MegaBios {
    public class SeatSelect {
        public static List<string> rowLetters = new() {"0", "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z"}; 
        private string _selectedSeat = "";
        // public bool FinishedSelectingSeats = false;
        public void SelectSeats(int rows, int columns, string movieName) {
            List<int> cursorPos = new() {1, 1};
            
            while (true) {
                DisplaySeats(rows, columns, movieName, cursorPos);
                cursorPos = NavigateMenu(cursorPos, rows, columns);
                Console.Clear();
            }
        }
        
        public void DisplaySeats(int rows, int columns, string movieName, List<int> cursorPos) {
            for (int i = 1; i <= rows; i++) {
                string rowLetter = rowLetters[i];
                System.Console.WriteLine("");
                for (int j = 1; j <= columns; j++) {
                    // Special color for handicapped seats
                    if (i == 1 && new List<int>() {0, 1, 2, columns, columns - 1, columns - 2}.Contains(j)) {
                        Console.ForegroundColor = ConsoleColor.Blue;
                    }
                    // If cursor is hovering over Seat:
                    if (i == cursorPos[1] && j == cursorPos[0]) {
                        _selectedSeat = rowLetter + Convert.ToString(j);
                        Console.BackgroundColor = ConsoleColor.Green;
                    }
                    

                    if (j == columns) {
                        Console.Write($"{rowLetter}{j}", Console.ForegroundColor, Console.BackgroundColor);
                    }
                    else {
                        Console.Write($"{rowLetter}{j}", Console.ForegroundColor, Console.BackgroundColor);
                        Console.ResetColor();
                        Console.Write(" ", Console.ForegroundColor, Console.BackgroundColor);
                    }
                    Console.ResetColor();
                }
            }
            System.Console.WriteLine("\n");
            PrintLegend();
            
        }

        private static List<int> NavigateMenu(List<int> cursor, int rows, int columns) {
            bool moved = false;
            while(!moved) {
                ConsoleKeyInfo keyInfo = Console.ReadKey(true);
                System.Console.WriteLine(keyInfo.Key);
                switch (keyInfo.Key) {
                    case ConsoleKey.UpArrow:
                        cursor[1]--;
                        cursor = CorrectCursorPos(cursor, rows, columns);
                        moved = true;
                        break;
                    case ConsoleKey.DownArrow:
                        cursor[1]++;
                        cursor = CorrectCursorPos(cursor, rows, columns);
                        moved = true;
                        break;
                    case ConsoleKey.LeftArrow:
                        cursor[0]--;
                        cursor = CorrectCursorPos(cursor, rows, columns);
                        moved = true;
                        break;
                    case ConsoleKey.RightArrow:
                        cursor[0]++;
                        cursor = CorrectCursorPos(cursor, rows, columns);
                        moved = true;
                        break;
                    
                    case ConsoleKey.Enter:
                        System.Console.WriteLine("You selected seat {rowLetter}{j}");
                        // FinishedSelectingSeats
                        break;
                }
            }
            return cursor;
            
        } 

        public static List<int> CorrectCursorPos(List<int> cursor, int rows, int columns) {
            if (cursor[0] < 1) {
                cursor[0] = 1;
            }
            else if (cursor[0] > columns) {
                cursor[0] = columns;
            }
            if (cursor[1] < 1) {
                cursor[1] = 1;
            }
            else if (cursor[1] > rows) {
                cursor[1] = rows;
            }
            return cursor;
        }

        public void PrintLegend() {
            System.Console.WriteLine($"Legend: ");
            Console.ForegroundColor = ConsoleColor.Blue;
            System.Console.Write("[]", Console.ForegroundColor);
            Console.ResetColor();
            System.Console.Write(" = Wheelchair seats, ");
            Console.ForegroundColor = ConsoleColor.Red;
            System.Console.Write("[]", Console.ForegroundColor);
            Console.ResetColor();
            System.Console.Write(" = Loveseats, [] = Regular seats\n");
            System.Console.WriteLine("\n");
        }
    }

}
