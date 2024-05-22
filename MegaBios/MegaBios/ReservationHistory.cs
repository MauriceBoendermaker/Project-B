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
        public DateTime ReservationRoom { get; set; }

        [JsonPropertyName("reservation_date")]
        public DateTime ReservationDate { get; set; }
        
        // TODO: Add property that saves the reserved seat(s) so it can be iterated over to unoccupy seats in case of ticket cancellation

        public void AddReservation(TestAccount account, ReservationHistory reservationHistory) {
        //
        }

        public bool IsReservationIDTaken(string reservationID) {
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

    }

    

}