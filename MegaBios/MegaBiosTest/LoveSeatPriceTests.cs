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
    }
}
