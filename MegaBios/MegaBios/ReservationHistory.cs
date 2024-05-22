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
        public List<string> ReservedSeats { get; set; }

        [JsonPropertyName("reservation_room")]
        public string ReservationRoom { get; set; }

        [JsonPropertyName("reservation_date")]
        public DateTime ReservationDate { get; set; }


        public ReservationHistory(string reservationNumber, string movieTitle, List<string> reservedSeats, string reservationRoom, DateTime reservationDate) {
            ReservationNumber = reservationNumber;
            MovieTitle = movieTitle;
            ReservedSeats = reservedSeats;
            ReservationRoom = reservationRoom;
            ReservationDate = reservationDate;
        } 

        // TODO: Add property that saves the reserved seat(s) so it can be iterated over to unoccupy seats in case of ticket cancellation

        public static void AddReservation(TestAccount account, ReservationHistory reservationHistory) {
            List<TestAccount> accounts = JsonFunctions.LoadCustomers("../../../customers.json");
            for (int i = 0; i < accounts.Count; i++) {
                if (accounts[i] == account) {
                    accounts[i].History.Add(reservationHistory);
                    JsonFunctions.WriteToJson("../../../customers.json", accounts);
                    break;
                } 
            }

        }   

        public static bool IsReservationIDTaken(string reservationID) {
            List<TestAccount> accounts = JsonFunctions.LoadCustomers("../../../customers.json");
            foreach(TestAccount account in accounts) {
                foreach(ReservationHistory reservation in account.History) {
                    if (reservation.ReservationNumber == reservationID) {
                        return true;
                    }
                }
            }
            return false;
        } 

        public static string generateReservasionNumber() {
            Random rand = new Random();
            while(true) {
                int reservationNumber = rand.Next(0, 10000);
                if (!IsReservationIDTaken(reservationNumber.ToString())) {
                    return reservationNumber.ToString();
                }
            }
            
        }
    }

    

}