using System.Text;
using System.Text.Json.Serialization;

namespace MegaBios
{
    public class Reservation
    {
        [JsonPropertyName("reservation_number")]
        public string ReservationNumber { get; set; }

        [JsonPropertyName("movie_title")]
        public string MovieTitle { get; set; }

        [JsonPropertyName("reserved_seats")]
        public List<Seat> ReservedSeats { get; set; }

        [JsonPropertyName("reservation_room")]
        public string ReservationRoom { get; set; }

        [JsonPropertyName("showing_date")]
        public DateTime ShowingDate { get; set; }

        [JsonPropertyName("date_of_reservation")]
        public DateTime ReservationDate { get; set; }
        [JsonPropertyName("discount")]
        public double Discount { get; set; }
        [JsonPropertyName("betaalwijze")]
        public string Betaalwijze { get; set; }

        public Reservation(string reservationNumber, string movieTitle, List<Seat> reservedSeats, string reservationRoom, DateTime showingDate, string betaalWijze,double discount = 0)
        {
            ReservationNumber = reservationNumber;
            MovieTitle = movieTitle;
            ReservedSeats = reservedSeats;
            ReservationRoom = reservationRoom;
            ShowingDate = showingDate;
            ReservationDate = DateTime.Now;
            Discount = discount;
            Betaalwijze = betaalWijze;
        }

        public static void AddReservation(Account account, Reservation reservation)
        {
            List<Account> accounts = JsonFunctions.LoadCustomers("../../../customers.json");

            for (int i = 0; i < accounts.Count; i++)
            {
                if (accounts[i] == account)
                {
                    accounts[i].Reservations.Add(reservation);
                    accounts[i].History.Add(reservation);

                    JsonFunctions.WriteToJson("../../../customers.json", accounts);

                    break;
                }
            }
        }

        public static void AddReservation(Guest guest, Reservation reservationHistory)
        {
            List<Guest> guests = JsonFunctions.LoadGuests("../../../guestreservations.json");

            for (int i = 0; i < guests.Count; i++)
            {
                if (guests[i] == guest)
                {
                    guests[i].Reservations.Add(reservationHistory);
                    JsonFunctions.WriteToJson("../../../guestreservations.json", guests);
                    break;
                }
            }
        }

        public static bool IsReservationIDTaken(string reservationID)
        {
            List<Account> accounts = JsonFunctions.LoadCustomers("../../../customers.json");

            foreach (Account account in accounts)
            {
                foreach (Reservation reservation in account.History)
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

        public static bool ConfirmPayment(Reservation reservation)
        {
            List<string> menuOptions = new() { "Ja", "Nee" };
            int selectedOption = MenuFunctions.Menu(menuOptions, PrintReservation(reservation));

            return selectedOption == 0 ? true : false;
        }

        public static StringBuilder PrintReservation(Reservation reservation)
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

            string roomString = reservation.ReservationRoom.Substring(0, 4) + " " + reservation.ReservationRoom.Substring(4);
            reservationPrint.AppendLine($"--------RESERVERING DATA-----------\n");
            reservationPrint.AppendLine($"Reservation Number: {reservation.ReservationNumber}");
            reservationPrint.AppendLine($"Film: {reservation.MovieTitle} \nRoom: {roomString}");
            reservationPrint.AppendLine($"Stoelen: \n{stoelenString}");
            reservationPrint.AppendLine($"Totaalprijs: {totalPrice:F2} Euro");
            reservationPrint.AppendLine($"Betaalwijze: {reservation.Betaalwijze}");

            if (reservation.Discount > 0) {
                reservationPrint.AppendLine($"Korting: {reservation.Discount * 100}%");
            }

            reservationPrint.AppendLine($"Tenstoonstellingsdatum: {reservation.ShowingDate}");
            reservationPrint.AppendLine("\nSelecteer \"ja\" om de bestelling te bevestigen\n");

            return reservationPrint;
        }

        public static string PrintReservationUser(Reservation reservation)
        {
            string stoelenString = "";
            double totalPrice = 0;
            Seat seat; // Declare seat buiten de loop

            for (int i = 0; i < reservation.ReservedSeats.Count; i++)
            {
                seat = reservation.ReservedSeats[i]; // Assign value in de loop
                stoelenString += $"{seat.SeatNumber}: {seat.Price:F2} Euro\n";
                totalPrice += seat.Price;
            }

            string roomString = reservation.ReservationRoom.Substring(0, 4) + " " + reservation.ReservationRoom.Substring(4);

            if (reservation.Discount > 0)
            {
                return $"Reserveringsnummer: {reservation.ReservationNumber}\n" +
                    $"Film: {reservation.MovieTitle}\n" +
                    $"Gereserveerde stoelen:\n{stoelenString}" +
                    $"Totaalprijs: {totalPrice:F2} Euro\n" +
                    $"Korting: {reservation.Discount*100}%" +
                    $"Betaalwijze: {reservation.Betaalwijze}" +
                    $"Reserveringszaal: {roomString}\n" +
                    $"Tenstoonstellingsdatum: {reservation.ShowingDate}\n" +
                    $"Bestellingsdatum: {reservation.ReservationDate}\n";

            }
            else
            {
                return $"Reserveringsnummer: {reservation.ReservationNumber}\n" +
                    $"Film: {reservation.MovieTitle}\n" +
                    $"Gereserveerde stoelen:\n{stoelenString}" +
                    $"Totaalprijs: {totalPrice:F2} Euro\n" + 
                    $"Betaalwijze: {reservation.Betaalwijze}" +
                    $"Reserveringszaal: {roomString}\n" +
                    $"Tenstoonstellingsdatum: {reservation.ShowingDate}\n" +
                    $"Bestellingsdatum: {reservation.ReservationDate}\n";
            }                
        }

        public static string PrintHistory(Reservation reservation)
        {
            string stoelenString = "";
            double totalPrice = 0;
            Seat seat; // Declare seat buiten de loop
            string roomString = reservation.ReservationRoom.Substring(0, 4) + " " + reservation.ReservationRoom.Substring(4);

            for (int i = 0; i < reservation.ReservedSeats.Count; i++)
            {
                seat = reservation.ReservedSeats[i]; // Assign value in de loop
                stoelenString += $"{seat.SeatNumber}: {seat.Price:F2} Euro\n";
                totalPrice += seat.Price;
            }

            if (reservation.Discount > 0)
            {
                return $"Reserveringsnummer: {reservation.ReservationNumber}\n" +
                    $"Film: {reservation.MovieTitle}\n" +
                    $"Gereserveerde stoelen:\n{stoelenString}" +
                    $"Totaalprijs: {totalPrice:F2} Euro\n" + // Gebruik stoelenString hier
                    $"Korting: {reservation.Discount*100}%" +
                    $"Reserveringszaal: {roomString}\n" +
                    $"Tenstoonstellingsdatum: {reservation.ShowingDate}\n" +
                    $"Bestellingsdatum: {reservation.ReservationDate}\n";
            }
            else
            {
                return $"Reserveringsnummer: {reservation.ReservationNumber}\n" +
                    $"Film: {reservation.MovieTitle}\n" +
                    $"Gereserveerde stoelen:\n{stoelenString}" +
                    $"Totaalprijs: {totalPrice:F2} Euro\n" + // Gebruik stoelenString hier
                    $"Reserveringszaal: {roomString}\n" +
                    $"Tenstoonstellingsdatum: {reservation.ShowingDate}\n" +
                    $"Bestellingsdatum: {reservation.ReservationDate}\n";
            } 
        }

        public static List<Seat> ApplyDiscount(List<Seat> selectedSeats, Account user)
        {
            double discount = 0;
            int leeftijd = DateTime.Now.Year - Convert.ToDateTime(user.GeboorteDatum).Year;

            if (user.IsStudent || leeftijd >= 65)
            {
                discount = 0.15;
            }
            
            for (int i = 0; i < selectedSeats.Count; i++)
            {
                selectedSeats[i].Price *= 1 - discount;
            }

            return selectedSeats;
        }

        public static double ReturnDiscount(Account user)
        {
            double discount = 0;
            int leeftijd = DateTime.Now.Year - Convert.ToDateTime(user.GeboorteDatum).Year;

            if (user.IsStudent || leeftijd >= 65)
            {
                discount = 0.15;
            }
            return discount;
        }
    }
}
