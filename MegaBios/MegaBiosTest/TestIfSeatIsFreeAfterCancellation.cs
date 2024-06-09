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
            // Arrange
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

            // Create a reserved seat
            Seat reservedSeat = new Seat
            {
                SeatNumber = "6-1",
                SeatTaken = true // Assume the seat is already reserved
            };
            // Create a list containing the reserved seat
            List<Seat> reservedSeatsList = new List<Seat> { reservedSeat };

            // Create a reservation with the reserved seat list
            Reservation reservation = new Reservation("12345", "Test Movie", reservedSeatsList, "Room 1", DateTime.Now);



            // Act
            Reservation updatedReservation = Program.CancelSeat(reservingAccount, reservation, reservedSeat);

            // Assert
            Assert.IsNotNull(updatedReservation);
            Assert.IsFalse(updatedReservation.ReservedSeats.Contains(reservedSeat)); // Check if the reserved seat is removed
            Assert.IsFalse(reservedSeat.SeatTaken); // Check if the seat becomes available again
        }
    }
}
