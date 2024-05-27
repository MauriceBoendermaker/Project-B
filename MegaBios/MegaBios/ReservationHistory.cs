using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace MegaBios
{
    public class ReservationHistory
    {
        [JsonPropertyName("reservation_number")]
        public string ReservationNumber { get; set; }

        [JsonPropertyName("movie_title")]
        public string MovieTitle { get; set; }

        [JsonPropertyName("reserved_seats")]
        public List<Seat> ReservedSeats { get; set; }

        [JsonPropertyName("reservation_room")]
        public string ReservationRoom { get; set; }

        [JsonPropertyName("reservation_date")]
        public DateTime ReservationDate { get; set; }
        // [JsonPropertyName("total_price")]
        // public double TotalPrice {get; set;}


        public ReservationHistory(string reservationNumber, string movieTitle, List<Seat> reservedSeats, string reservationRoom, DateTime reservationDate)
        {
            ReservationNumber = reservationNumber;
            MovieTitle = movieTitle;
            ReservedSeats = reservedSeats;
            ReservationRoom = reservationRoom;
            ReservationDate = reservationDate;
            // TotalPrice = totalPrice;
        }

        // TODO: Add property that saves the reserved seat(s) so it can be iterated over to unoccupy seats in case of ticket cancellation

        public static void AddReservation(TestAccount account, ReservationHistory reservationHistory)
        {
            List<TestAccount> accounts = JsonFunctions.LoadCustomers("../../../customers.json");
            for (int i = 0; i < accounts.Count; i++)
            {
                if (accounts[i] == account)
                {
                    accounts[i].History.Add(reservationHistory);
                    JsonFunctions.WriteToJson("../../../customers.json", accounts);
                    break;
                }
            }

        }

        public static void AddReservation(Guest guest, ReservationHistory reservationHistory)
        {
            List<Guest> guests = JsonFunctions.LoadGuests("../../../guestreservations.json");
            for (int i = 0; i < guests.Count; i++)
            {
                if (guests[i] == guest)
                {
                    guests[i].History.Add(reservationHistory);
                    JsonFunctions.WriteToJson("../../../guestreservations.json", guests);
                    break;
                }
            }
        }

        public static bool IsReservationIDTaken(string reservationID)
        {
            List<TestAccount> accounts = JsonFunctions.LoadCustomers("../../../customers.json");
            foreach (TestAccount account in accounts)
            {
                foreach (ReservationHistory reservation in account.History)
                {
                    if (reservation.ReservationNumber == reservationID)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public static string generateReservationNumber()
        {
            Random rand = new Random();
            while (true)
            {
                int reservationNumber = rand.Next(0, 10000);
                if (!IsReservationIDTaken(reservationNumber.ToString()))
                {
                    return reservationNumber.ToString();
                }
            }

        }

        public static bool ConfirmPayment(ReservationHistory reservation)
        {
            List<string> menuOptions = new() { "Ja", "Nee" };
            int selectedOption = MenuFunctions.Menu(menuOptions, PrintReservation(reservation));
            return selectedOption == 0 ? true : false;
        }

        public static StringBuilder PrintReservation(ReservationHistory reservation)
        {
            StringBuilder reservationPrint = new StringBuilder();
            string stoelenString = "";
            double totalPrice = 0;
            for (int i = 0; i < reservation.ReservedSeats.Count; i++)
            {
                Seat seat = reservation.ReservedSeats[i];
                stoelenString += $"{seat.SeatNumber}: {seat.Price:F2} Euro\n";
                totalPrice += seat.Price;
            }
            reservationPrint.AppendLine($"--------RESERVERING DATA-----------\n");
            reservationPrint.AppendLine($"Reservation Number: {reservation.ReservationNumber}");
            reservationPrint.AppendLine($"Film: {reservation.MovieTitle} Room: {reservation.ReservationRoom}");
            reservationPrint.AppendLine($"Stoelen: \n{stoelenString}");
            reservationPrint.AppendLine($"Totaalprijs: {totalPrice} Euro");
            reservationPrint.AppendLine("\nSelecteer \"ja\" om de bestelling te bevestigen\n");
            return reservationPrint;
        }

        public static string PrintReservationUser(ReservationHistory reservation)
        {
            string stoelenString = "";
            double totalPrice = 0;
            Seat seat; // Declare seat outside the loop
            for (int i = 0; i < reservation.ReservedSeats.Count; i++)
            {
                seat = reservation.ReservedSeats[i]; // Assign value inside the loop
                stoelenString += $"{seat.SeatNumber}: {seat.Price:F2} Euro\n";
                totalPrice += seat.Price;
            }
            return $"--------UW BESTELLINGEN-----------\n\n" +
                $"Reserveringsnummer: {reservation.ReservationNumber}\n" +
                $"Film: {reservation.MovieTitle}\n" +
                $"Gereserveerde stoelen:\n{stoelenString}" +
                $"Totaalprijs: {totalPrice} Euro\n" + // Use stoelenString here
                $"Reserveringszaal: {reservation.ReservationRoom}\n" +
                $"Reserveringsdatum: {reservation.ReservationDate}";
        }

        public static List<Seat> ApplyDiscount(List<Seat> selectedSeats, TestAccount user)
        {
            double discount = 0;
            if (user.IsStudent)
            {
                discount = 0.15;
            }
            for (int i = 0; i < selectedSeats.Count; i++)
            {
                selectedSeats[i].Price *= 1 - discount;
            }
            return selectedSeats;
        }

        public static List<Seat> ApplyDiscount(List<Seat> selectedSeats, Guest guest)
        {
            double discount = 0;
            if (guest.IsStudent)
            {
                discount = 0.15;
            }
            for (int i = 0; i < selectedSeats.Count; i++)
            {
                selectedSeats[i].Price *= 1 - discount;
            }
            return selectedSeats;
        }
    }



}