using MegaBios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MegaBiosTest
{
    [TestClass]
    public class SeatCancellationTests
    {
        [TestMethod]
        public void CancelSeat_SeatBecomesAvailable()
        {
            Account reservingAccount = new Account(
                "Daan",
                "", // Tussenvoegsel
                "Bakker",
                "1975-04-20",
                new Dictionary<string, string>
                {
        { "straat", "Voorstraat" },
        { "huisnummer", "45" },
        { "woonplaats", "Rotterdam" },
        { "postcode", "3011 HN" }
                },
                "daan.bakker@example.com",
                "G3heim!",
                "+31 10 98765432",
                "Creditcard",
                false,
                new List<Reservation>(),
                new List<Reservation>()
            );

            Seat reservedSeat = new Seat
            {
                SeatNumber = "6-1",
                SeatTaken = true 
            };
            List<Seat> reservedSeatsList = new List<Seat> { reservedSeat };

            Reservation reservation = new Reservation("12345", "Test Movie", reservedSeatsList, "Room 1", DateTime.Now);



            Reservation updatedReservation = Program.CancelSeat(reservingAccount, reservation, reservedSeat);

            Assert.IsNotNull(updatedReservation);
            Assert.IsFalse(updatedReservation.ReservedSeats.Contains(reservedSeat)); // Check if the reserved seat is removed
            Assert.IsFalse(reservedSeat.SeatTaken); // Check if the seat becomes available again
        }
    }
}
