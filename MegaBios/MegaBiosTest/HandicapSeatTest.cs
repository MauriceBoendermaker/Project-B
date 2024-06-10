using MegaBios;

namespace MegaBiosTest
{
    [TestClass]
    public class HandicapSeatTest
    {
        [TestMethod]
        public void CheckForHandicapSeats()
        {
            // Arrange
            var roomShowings = new List<RoomShowing>(); // Valid room showings
            string roomNumber = "Room 1";
            DateTime showTime = DateTime.Now;

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

            List<List<Seat>> seating = new List<List<Seat>> {
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
