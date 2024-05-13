using System.Formats.Asn1;
using System.Reflection.Metadata;
using System.Text;

namespace MegaBios
{
    public class SeatSelect
    {
        public static List<string> rowLetters = new() { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z" };
        private List<Seat> _selectedSeats = new();
        private bool _confirmedSeats = false;
        private Seat _selectedSeat;
        private int _selectedSeatsRow = -1;
        private int _selectedSeatsLeftBound = -1;
        private int _selectedSeatsRightBound = -1;
        private string _extraMessage;

        public List<List<Seat>> Seats = new List<List<Seat>>();


        public List<RoomShowing> RoomShowings {get; set;}
        public RoomShowing Showing {get; set;}
        public string RoomNumber {get; set;}
        public DateTime ShowTime {get; set;}
        // public bool FinishedSelectingSeats = false;

        public SeatSelect(List<RoomShowing> roomShowings, string roomNumber, DateTime showTime)
        {
            RoomShowings = roomShowings;
            RoomNumber = roomNumber;
            ShowTime = showTime;
            foreach (RoomShowing showing in roomShowings) {
                if (showing.ShowingTime == showTime) {
                    Seats = showing.Seating;
                    Showing = showing;
                }
            }
        }

        private Seat GetCorrespondingLoveSeatRight(Seat loveSeat)
        {
            // Get the row and seat number of the selected love seat
            int rowIndex = rowLetters.IndexOf(loveSeat.SeatNumber.Substring(0, 1));
            int seatIndex = int.Parse(loveSeat.SeatNumber.Substring(1)) - 1;

            // Get the corresponding seat number
            string correspondingSeatNumber = $"{rowLetters[rowIndex]}{seatIndex + 2}";

            // Find and return the corresponding seat
            foreach (List<Seat> row in Seats)
            {
                foreach (Seat seat in row)
                {
                    if (seat.SeatNumber == correspondingSeatNumber)
                    {
                        return seat;
                    }
                }
            }

            // If no corresponding seat is found, return null
            return null;
        }
        private Seat GetCorrespondingLoveSeatLeft(Seat loveSeat)
        {
            // Get the row and seat number of the selected love seat
            int rowIndex = rowLetters.IndexOf(loveSeat.SeatNumber.Substring(0, 1));
            int seatIndex = int.Parse(loveSeat.SeatNumber.Substring(1)) - 1;

            // Get the corresponding seat number
            string correspondingSeatNumber = $"{rowLetters[rowIndex]}{seatIndex}";

            // Find and return the corresponding seat
            foreach (List<Seat> row in Seats)
            {
                foreach (Seat seat in row)
                {
                    if (seat.SeatNumber == correspondingSeatNumber)
                    {
                        return seat;
                    }
                }
            }

            // If no corresponding seat is found, return null
            return null;
        }

        public void MarkSeatsAsSelected() {
            // Get all the seat numbers from the selectedseats and add them into a list
            List<string> seatNumbers = new();
            foreach(Seat selectedSeat in _selectedSeats) {
                seatNumbers.Add(selectedSeat.SeatNumber);
            }

            // Iterate over 2D List and mark each selected seat as selected in said 2D list.
            for (int i = 0; i < Seats.Count; i++) {
                for (int j = 0; j < Seats[i].Count; j++) {
                    if (seatNumbers.Contains(Seats[i][j].SeatNumber)) {
                        Seats[i][j].SeatTaken = true;
                    }
                }
            }
            UpdateRoomSeating();
        }

        public void UpdateRoomSeating() {
            for (int i = 0; i < RoomShowings.Count; i++) {
                if (RoomShowings[i].ShowingTime == ShowTime) {
                    RoomShowings[i].Seating = Seats;
                    break;
                }
            }
            JsonFunctions.WriteToJson($"../../../{RoomNumber}.json", RoomShowings);
        }

        public void SelectSeats()
        {
            List<int> cursorPos = new() { 1, 1 };
            DisplaySeats(cursorPos);
            while (!_confirmedSeats)
            {
                Console.Clear();
                DisplaySeats(cursorPos);
                cursorPos = NavigateMenu(cursorPos);
            }
            Console.WriteLine($"Stoelen geselecteerd:");
            string selectedSeatsString = "";
            foreach (Seat seat in _selectedSeats)
            {
                selectedSeatsString += seat.SeatNumber + " ";
            }
            Console.WriteLine(selectedSeatsString);
            MarkSeatsAsSelected();
        }

        public void DisplaySeats(List<int> cursorPos)
        {
            List<List<Seat>> seating = Seats;

            Console.Clear();
            Console.WriteLine("\n\x1b[0m");

            StringBuilder seatingText = new StringBuilder();

            double currentSeatPrice = 0.0;

            for (int i = 0; i < seating.Count; i++)
            {
                string rowLetter = rowLetters[i];
                for (int j = 0; j < seating[i].Count; j++)
                {
                    string colorText = ""; // ANSI kleurcode string
                    if (seating[i][j].SeatType == "handicap")
                    {
                        colorText = "\x1b[34m"; // Blauw
                    }
                    else if (seating[i][j].SeatType == "love seat")
                    {
                        colorText = "\x1b[35m"; // Magenta
                    }
                    if (seating[i][j].SeatTaken)
                    {
                        colorText += "\x1b[41m"; // Rode kleur voor bezette stoelen
                    }
                    else if (i == cursorPos[1] && j == cursorPos[0])
                    {
                        _selectedSeat = seating[i][j];
                        currentSeatPrice = _selectedSeat.Price; // Update huidige stoel
                        colorText += "\x1b[43m"; // Gele achtergrond voor huidige stoel
                    }
                    else if (_selectedSeats.Contains(seating[i][j]))
                    {
                        colorText += "\x1b[42m"; // Groene achtergrond voor al bezette stoelen
                    }
                    seatingText.Append($"{colorText}{seating[i][j].SeatNumber}\x1b[0m ");
                }
                seatingText.AppendLine("\x1b[0m");
            }

            Console.WriteLine(seatingText);
            Console.WriteLine("\nGeselecteerde stoelen: ");
            _selectedSeats.ForEach(seat => Console.Write(seat.SeatNumber + " "));

            double totalPrice = CalculateTotalPrice();

            Console.WriteLine($"\n\nHuidige stoel prijs: {currentSeatPrice:0.00} euro");
            Console.WriteLine($"Totaalprijs van geselecteerde stoelen: {totalPrice:0.00} euro\n"); // Totaalprijs

            PrintLegend();

            Console.WriteLine("\nDruk op pijltoetsen om te navigeren. Druk op Space om stoel te selecteren. Druk op enter om stoelselectie te bevestigen. Druk op Backspace om stoelselectie te wissen");
            Console.WriteLine(_extraMessage);
        }

        private List<int> NavigateMenu(List<int> cursor)
        {
            bool moved = false;
            List<List<Seat>> seating = Seats;
            while (!moved)
            {
                ConsoleKeyInfo keyInfo = Console.ReadKey(true);
                switch (keyInfo.Key)
                {
                    // Cursor voor navigeren, enter is select huidige stoel.
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
                        if (_selectedSeats.Contains(_selectedSeat))
                        {
                            _extraMessage = "Je hebt deze stoel al geselecteerd!";
                        }
                        else if (_selectedSeat.SeatTaken == true)
                        {
                            _extraMessage = "Deze stoel is al bezet!";
                        }
                        else if (isAdjacent)
                        {
                            _selectedSeats.Add(_selectedSeat);
                            UpdateSeatBounds(_selectedSeat);
                            if (_selectedSeat.SeatType == "love seat")
                            {
                                Seat rightCorrespondingSeat = GetCorrespondingLoveSeatRight(_selectedSeat);
                                Seat leftCorrespondingSeat = GetCorrespondingLoveSeatLeft(_selectedSeat);

                                if (rightCorrespondingSeat != null && !_selectedSeats.Contains(rightCorrespondingSeat) && rightCorrespondingSeat.SeatType == "love seat")
                                {
                                    _selectedSeats.Add(rightCorrespondingSeat);
                                    UpdateSeatBounds(rightCorrespondingSeat);
                                }
                                if (leftCorrespondingSeat != null && !_selectedSeats.Contains(leftCorrespondingSeat) && leftCorrespondingSeat.SeatType == "love seat")
                                {
                                    _selectedSeats.Add(leftCorrespondingSeat);
                                    UpdateSeatBounds(leftCorrespondingSeat);
                                }
                            }
                            _extraMessage = $"Stoel geselecteerd: {_selectedSeat.SeatNumber}";
                        }
                        else if (!isAdjacent)
                        {
                            _extraMessage = "Deze stoel is niet aangrenzend aan je huidige geselecteerde stoelen. Selecteer alstublieft een aangrenzende stoel.";
                        }
                        break;
                    case ConsoleKey.Enter:
                        moved = true;
                        if (_selectedSeats.Count == 0)
                        {
                            _extraMessage = "Je hebt nog geen stoelen geselecteerd";
                            break;
                        }
                        else if (_selectedSeats.Count > 0)
                        {
                            System.Console.WriteLine("Weet je zeker dat je deze stoelen wilt selecteren? Druk nogmaals op enter om je keuze te bevestigen of een andere knop om terug te gaan.");
                            if (Console.ReadKey(true).Key == ConsoleKey.Enter)
                            {
                                _confirmedSeats = true;
                                break;
                            }
                        }
                        break;
                    case ConsoleKey.Backspace:
                        moved = true;
                        if (_selectedSeats.Count == 0)
                        {
                            _extraMessage = "Je hebt nog geen stoelen geselecteerd";
                            break;
                        }
                        else if (_selectedSeats.Count > 0)
                        {
                            System.Console.WriteLine("Weet je zeker dat je je selectie wil wissen? Druk nogmaals op Backspace om je selectie te wissen.");
                            if (Console.ReadKey(true).Key == ConsoleKey.Backspace)
                            {
                                _selectedSeats.Clear();
                                _selectedSeatsLeftBound = -1;
                                _selectedSeatsRightBound = -1;
                                _selectedSeatsRow = -1;
                                break;
                            }
                        }
                        break;
                    default:
                        System.Console.WriteLine("Druk alstublieft op pijltoetsen, space, enter of backspace");
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
            else if (cursor[0] >= columns)
            {
                cursor[0] = columns - 1;
            }

            if (cursor[1] < 0)
            {
                cursor[1] = 0;
            }
            else if (cursor[1] >= rows)
            {
                cursor[1] = rows - 1;
            }
            return cursor;
        }

        public void PrintLegend()
        {
            StringBuilder legendText = new StringBuilder();
            legendText.Append($"Legenda:\n");
            legendText.Append("\x1b[34m[]\x1b[0m = Handicap Stoelen (10.00 euro), \x1b[35m[]\x1b[0m = Loveseats (20.00 euro), [] = Normale Stoelen (10.00 euro), \x1b[41m  \x1b[0m = Bezette Stoelen, \x1b[42m  \x1b[0m = Gekozen Stoelen, \x1b[43m  \x1b[0m = Huidige Stoel");
            System.Console.WriteLine(legendText);

        }

        public void UpdateSeatBounds(Seat seat)
        {
            int seatIndex = int.Parse(seat.SeatNumber.Substring(1)) - 1;
            if (seatIndex < _selectedSeatsLeftBound)
            {
                _selectedSeatsLeftBound = seatIndex;
            }
            else if (seatIndex > _selectedSeatsRightBound)
            {
                _selectedSeatsRightBound = seatIndex;
            }
        }

        public bool AdjacentSeatCheck(List<int> cursor)
        {
            // If no seat has been selected prior
            if (_selectedSeats.Count == 0)
            {
                _selectedSeatsRow = cursor[1];
                _selectedSeatsLeftBound = cursor[0];
                _selectedSeatsRightBound = cursor[0];
                return true;
            }
            // Check if seat selected is on the right row and if the selected seat is adjacent to the currnet selected seats or not
            else if (cursor[1] == _selectedSeatsRow)
            {
                if (Math.Abs(cursor[0] - _selectedSeatsRightBound) > 1 && Math.Abs(cursor[0] - _selectedSeatsLeftBound) > 1)
                {
                    return false;
                }
                else if (Math.Abs(cursor[0] - _selectedSeatsRightBound) == 1 || Math.Abs(cursor[0] - _selectedSeatsLeftBound) == 1)
                {
                    return true;
                }
                return false;
            }
            else
            {
                return false;
            }
        }

        public double CalculateTotalPrice()
        {
            double total = 0.0;

            foreach (Seat seat in _selectedSeats)
            {
                total += seat.Price;
            }

            return total;
        }
    }
}
