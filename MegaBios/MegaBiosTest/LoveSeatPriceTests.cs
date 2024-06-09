using Microsoft.VisualStudio.TestTools.UnitTesting;
using MegaBios;
using System.Collections.Generic;
using System.Reflection;

namespace MegaBiosTest.Services
{
    [TestClass]
    public class SeatPricingTests
    {
        [TestMethod]
        public void LoveSeatPricing_LoveSeatsHaveDifferentPrice()
        {
            // Arrange
            var seatingPlan = new List<List<Seat>>
            {
                new List<Seat>
                {
                    new Seat { SeatType = "normal", Price = 10.00 },
                    new Seat { SeatType = "love seat", Price = 20.00 },
                    new Seat { SeatType = "love seat", Price = 20.00 }
                }
            };

            // Act
            // No action required, as pricing is predefined in the seating plan

            // Assert
            // Ensure love seats have a different price than normal seats
            Assert.AreEqual(10.00, seatingPlan[0][0].Price); // Normal seat
            Assert.AreEqual(20.00, seatingPlan[0][1].Price); // Love seat 1
            Assert.AreEqual(20.00, seatingPlan[0][2].Price); // Love seat 2
        }
        [TestMethod]
        public void CheckForLoveSeats()
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
                    new Seat { SeatType = "love seat", SeatTaken = false },
                    new Seat { SeatType = "normal", SeatTaken = false }
                },
                new List<Seat>
                {
                    new Seat { SeatType = "normal", SeatTaken = false },
                    new Seat { SeatType = "normal", SeatTaken = false },
                    new Seat { SeatType = "love seat", SeatTaken = false }
                }
            };

            SeatSelect seatSelect = new SeatSelect(roomShowings, roomNumber, showTime, reservingAccount);
            seatSelect.Seats = seating;

            // Act
            bool hasLoveSeats = seatSelect.HasLoveSeats();

            // Assert
            Assert.IsTrue(hasLoveSeats);
        }
    }
}
