using MegaBios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MegaBiosTest
{
    [TestClass]

    public class SeatAvailableAfterCancellation
    {
        private string filePath = "../../../Test_Room1.json";

        [TestInitialize]
        public void TestInitialize()
        {
            // Stel de omgevingsvariabele in voor de testomgeving
            Environment.SetEnvironmentVariable("IS_TEST_ENVIRONMENT", "true");

            // Maak een leeg bestand aan als het origineel niet bestaat
            if (!File.Exists(filePath))
            {
                File.WriteAllText(filePath, "[]");
            }
        }

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
                SeatTaken = true,
                Price = 10,
                SeatType = "normal"
            };
            List<Seat> reservedSeatsList = new List<Seat> { reservedSeat };

            Reservation reservation = new Reservation("12345", "testfilm", reservedSeatsList, "Test_Room1", Convert.ToDateTime("2024-08-25T09:00:00"));
            SeatSelect.MarkSeatsAsSelected(reservedSeatsList, reservation.ShowingDate, reservation.ReservationRoom);



            Reservation updatedReservation = Program.CancelSeat(reservingAccount, reservation, reservedSeat);

            Assert.IsNotNull(updatedReservation);
            Assert.IsFalse(updatedReservation.ReservedSeats.Contains(reservedSeat)); // Controleer of de gereserveerde seat is verwijderd
        }
    }
}
