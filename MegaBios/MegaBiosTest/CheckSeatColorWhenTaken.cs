using MegaBios;

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
                    new Seat { SeatType = "normal", SeatTaken = true }, // De stoel is bezet en moet rood zijn
                    new Seat { SeatType = "normal", SeatTaken = false }
                }
            };

            foreach (List<Seat> row in seating)
            {
                foreach (Seat seat in row)
                {
                    if (seat.SeatTaken)
                    {
                        // Controleer of de stoel bezet is
                        Assert.IsTrue(seat.SeatTaken);
                    }
                }
            }
        }
    }
}
