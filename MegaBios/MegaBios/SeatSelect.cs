using System.Formats.Asn1;
using System.Reflection.Metadata;

namespace MegaBios
{
    public class SeatSelect
    {
        public static List<string> rowLetters = new() {"A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z" };
        private List<Seat> _selectedSeats = new() {};
        private bool _confirmedSeats = false;
        private Seat _selectedSeat;
        private int _selectedSeatsRow = -1;
        private int _selectedSeatsLeftBound = -1;
        private int _selectedSeatsRightBound = -1;
        private string _extraMessage;
        

        public CinemaRoom Room {get; set;}
        // public bool FinishedSelectingSeats = false;

        public SeatSelect(CinemaRoom room) {
            Room = room;
        }

        public void SelectSeats()
        {
            List<int> cursorPos = new() { 1, 1 };
            DisplaySeats(Room, cursorPos);
            while (!_confirmedSeats)
            {
                Console.Clear();
                DisplaySeats(Room, cursorPos);
                cursorPos = NavigateMenu(cursorPos);
            }
            System.Console.WriteLine($"Final seat selection:");
            foreach(Seat seat in _selectedSeats) {
                System.Console.WriteLine(seat.SeatNumber);
            }
        }


        public void DisplaySeats(CinemaRoom room, List<int> cursorPos)
        {
            List<List<Seat>> seating = room.Seating;
            System.Console.WriteLine("\n\n");
            for (int i = 0; i < seating.Count; i++)
            {
                string rowLetter = rowLetters[i];
                System.Console.WriteLine("");

                for (int j = 0; j < seating[i].Count; j++)
                {
                    // Special color for handicapped seats
                    if (seating[i][j].SeatTaken == true) {
                        Console.BackgroundColor = ConsoleColor.Red;
                    }

                    else if (seating[i][j].SeatType == "handicap")
                    {
                        Console.ForegroundColor = ConsoleColor.Blue;
                    }

                    // Special color for love seat
                    else if (seating[i][j].SeatType == "love seat") {
                        Console.ForegroundColor = ConsoleColor.Magenta;
                    }

                    // If cursor is hovering over Seat or or if seat is already selected:
                    if ((i == cursorPos[1] && j == cursorPos[0]) || _selectedSeats.Contains(seating[i][j]))
                    {
                        // Adds the selected seat to the list
                        if ((i == cursorPos[1] && j == cursorPos[0])) {
                            _selectedSeat = seating[i][j];
                        }
                        Console.BackgroundColor = ConsoleColor.Green;
                    }

                    // Print misc chairs
                    if (j == seating[i].Count)
                    {
                        Console.Write($"{seating[i][j].SeatNumber}", Console.ForegroundColor, Console.BackgroundColor);
                    }
                    else
                    {
                        Console.Write($"{seating[i][j].SeatNumber}", Console.ForegroundColor, Console.BackgroundColor);
                        Console.ResetColor();
                        Console.Write(" ", Console.ForegroundColor, Console.BackgroundColor);
                    }
                    Console.ResetColor();
                }
            }
            System.Console.WriteLine("\n");
            System.Console.Write("Selected seats: ");
            string selectedSeatsString = "";
            foreach (Seat seat in _selectedSeats) {
                selectedSeatsString += seat.SeatNumber+ " ";
            }
            System.Console.Write(selectedSeatsString);
            System.Console.WriteLine("\n");
            PrintLegend();
            System.Console.WriteLine("\nPress arrow keys to navigate. Press Space to select seat. Press enter to confirm tickets. Press Backspace to clear selection");
            System.Console.WriteLine(_extraMessage);
        }

        private List<int> NavigateMenu(List<int> cursor)
        {
            bool moved = false;
            List<List<Seat>> seating = Room.Seating;
            while (!moved)  
            {
                ConsoleKeyInfo keyInfo = Console.ReadKey(true);
                switch (keyInfo.Key)
                {
                    // The cases with up-down-left-right arrow adjust the cursor accordingly. The enter button selects the current seat
                    case ConsoleKey.UpArrow:
                        cursor[1]--;
                        cursor = CorrectCursorPos(cursor, seating.Count, seating[0].Count);
                        moved = true;
                        break;
                    case ConsoleKey.DownArrow:
                        cursor[1]++;
                        cursor = CorrectCursorPos(cursor, seating.Count, seating[0].Count);
                        moved = true;
                        break;
                    case ConsoleKey.LeftArrow:
                        cursor[0]--;
                        cursor = CorrectCursorPos(cursor, seating.Count, seating[0].Count);
                        moved = true;
                        break;
                    case ConsoleKey.RightArrow:
                        cursor[0]++;
                        cursor = CorrectCursorPos(cursor, seating.Count, seating[0].Count);
                        moved = true;
                        break;
                    case ConsoleKey.Spacebar:
                        moved = true;
                        bool isAdjacent = AdjacentSeatCheck(cursor);
                        if (_selectedSeats.Contains(_selectedSeat)) {
                            // System.Console.WriteLine("You have already selected that seat!");
                            _extraMessage = "Je hebt deze stoel al geselecteerd!";
                        }

                        else if (_selectedSeat.SeatTaken == true) {
                            // System.Console.WriteLine("Seat is already taken!");
                            _extraMessage = "Deze stoel is al bezet!";
                        }

                        else if (isAdjacent) {
                            _selectedSeats.Add(_selectedSeat);
                            if (cursor[0] < _selectedSeatsLeftBound) {
                                _selectedSeatsLeftBound = cursor[0];
                            }
                            else if (cursor[0] > _selectedSeatsRightBound) {
                                _selectedSeatsRightBound = cursor[0];
                            }
                            // System.Console.WriteLine($"You selected seat {_selectedSeat.SeatNumber}");
                            _extraMessage = $"You selected seat {_selectedSeat.SeatNumber}";
                        }

                        else if (!isAdjacent) {
                            // System.Console.WriteLine("Seat is not adjacent to your currently selected seat(s). Please select an adjacent seat");
                            _extraMessage = "Deze stoel is niet aangrenzend aan je huidige geselecteerde stoelen. Selecteer alstublieft een aangrenzende stoel.";
                        }

                        // if (_selectedSeats.Count == 0) {
                        //     System.Console.WriteLine("Press any button (except enter) to continue selecting seats");
                        //     keyInfo = Console.ReadKey(true);
                        //     break;
                        // 
                        // FinishedSelectingSeats
                        break;
                    case ConsoleKey.Enter:
                        moved = true;
                        if (_selectedSeats.Count == 0) {
                            _extraMessage = "Je hebt nog geen stoelen geselecteerd";
                            break;
                        }
                        else if (_selectedSeats.Count > 0){
                            System.Console.WriteLine("Weet je zeker dat je deze stoelen wilt selecteren? Druk nogmaals op enter om je keuze te bevestigen of een andere knop om terug te gaan.");
                            if (Console.ReadKey(true).Key == ConsoleKey.Enter) {
                                _confirmedSeats = true;
                                break;
                            }
                        }
                        break;
                    case ConsoleKey.Backspace:
                        moved = true;
                        if (_selectedSeats.Count == 0) {
                            _extraMessage = "Je hebt nog geen stoelen geselecteerd";
                            break;
                        }
                        else if (_selectedSeats.Count > 0){
                            System.Console.WriteLine("Weet je zeker dat je je selectie wil wissen? Druk nogmaals op Backspace om je selectie te wissen.");
                            if (Console.ReadKey(true).Key == ConsoleKey.Backspace) {
                                _selectedSeats.Clear();
                                _selectedSeatsLeftBound = -1;
                                _selectedSeatsRightBound = -1;
                                _selectedSeatsRow = -1;
                                break;
                            }
                        }
                        break;
                    default:
                        System.Console.WriteLine("Please press the arrow keys, space or enter");
                        break;
                }
            }
            return cursor;
        }

        // Adjusts the cursor position to make sure it does not go out of the seating boundaries.
        public static List<int> CorrectCursorPos(List<int> cursor, int rows, int columns)
        {
            if (cursor[0] < 0)
            {
                cursor[0] = 0;
            }
            else if (cursor[0] > columns)
            {
                cursor[0] = columns;
            }

            if (cursor[1] < 0)
            {
                cursor[1] = 0;
            }
            else if (cursor[1] > rows)
            {
                cursor[1] = rows;
            }
            return cursor;
        }

        public void PrintLegend()
        {
            System.Console.WriteLine($"Legend: ");
            Console.ForegroundColor = ConsoleColor.Blue;

            System.Console.Write("[]", Console.ForegroundColor);
            Console.ResetColor();

            System.Console.Write(" = Wheelchair seats, ");
            Console.ForegroundColor = ConsoleColor.Magenta;

            System.Console.Write("[]", Console.ForegroundColor);
            Console.ResetColor();

            System.Console.Write(" = Loveseats, [] = Regular seats, ");
            Console.BackgroundColor = ConsoleColor.Red;

            System.Console.Write("[]", Console.BackgroundColor);
            Console.ResetColor();

            System.Console.Write(" = Taken seats, ");
            Console.BackgroundColor = ConsoleColor.Green;

            System.Console.Write("[]", Console.BackgroundColor);
            Console.ResetColor();

            System.Console.Write(" = Chosen seats\n");
            System.Console.WriteLine("\n");

        }

        public bool AdjacentSeatCheck(List<int> cursor) {
            // If no seat has been selected prior
            if (_selectedSeats.Count == 0 ) {
                _selectedSeatsRow = cursor[1];
                _selectedSeatsLeftBound = cursor[0];
                _selectedSeatsRightBound = cursor[0];
                return true;
            }
            // Check if seat selected is on the right row and if the selected seat is adjacent to the currnet selected seats or not
            else if (cursor[1] == _selectedSeatsRow) {
                if (Math.Abs(cursor[0] - _selectedSeatsRightBound) > 1 && Math.Abs(cursor[0] - _selectedSeatsLeftBound) > 1) {
                    return false;
                }
                else  if (Math.Abs(cursor[0] - _selectedSeatsRightBound) == 1 || Math.Abs(cursor[0] - _selectedSeatsLeftBound) == 1) {
                    return true;
                }
                return false;
            }
            else {
                return false;
            }
        }
    }
}