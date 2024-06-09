using MegaBios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MegaBiosTest
{
    [TestClass]
    public class HandicapSeatSelectTests
    {
        [TestMethod]
        public void CheckForHandicapSeats()
        {
            // Arrange
            var roomShowings = new List<RoomShowing>(); // Provide valid room showings
            string roomNumber = "Room 1"; // Provide a valid room number
            DateTime showTime = DateTime.Now; // Provide a valid show time
            Account reservingAccount = new Account(
                "Daan",
                "", // Tussenvoegsel (middle name) seems to be empty in your example
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



            // Mock or provide a valid account instance
            List<List<Seat>> seating = new List<List<Seat>>
    {
        new List<Seat>
        {
            new Seat { SeatType = "normal", SeatTaken = false },
            new Seat { SeatType = "handicap", SeatTaken = false },
            new Seat { SeatType = "normal", SeatTaken = false }
        },
        new List<Seat>
        {
            new Seat { SeatType = "normal", SeatTaken = false },
            new Seat { SeatType = "normal", SeatTaken = false },
            new Seat { SeatType = "handicap", SeatTaken = false }
        }
    };

            SeatSelect seatSelect = new SeatSelect(roomShowings, roomNumber, showTime, reservingAccount);
            seatSelect.Seats = seating;

            // Act
            bool hasHandicapSeats = seatSelect.HasHandicapSeats();

            // Assert
            Assert.IsTrue(hasHandicapSeats);
        }

    }
}
