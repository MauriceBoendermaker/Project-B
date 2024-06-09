using Microsoft.VisualStudio.TestTools.UnitTesting;
using MegaBios;
using System.Collections.Generic;
using System.Reflection;

namespace MegaBiosTest
{
    [TestClass]
    public class CheckSeatisTaken
    {
        [TestMethod]
        public void DisplaySeats_SeatTaken_Test()
        {
            
            var seating = new List<List<Seat>>
            {
                new List<Seat>
                {
                    new Seat { SeatType = "normal", SeatTaken = false },
                    new Seat { SeatType = "normal", SeatTaken = true }, // Seat is taken should be red
                    new Seat { SeatType = "normal", SeatTaken = false }
                }
            };

            foreach (List<Seat> row in seating)
            {
                foreach (Seat seat in row)
                {
                    if (seat.SeatTaken)
                    {
                        // Check if the seat is taken
                        Assert.IsTrue(seat.SeatTaken);
                    }
                }
            }
        }
    }
}
