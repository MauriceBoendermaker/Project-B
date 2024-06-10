using MegaBios;

namespace MegaBiosTest.Services
{
    [TestClass]
    public class ReservationTests
    {
        [TestMethod]
        public void ApplyDiscount_Student_ReturnsDiscountedSeats()
        {
            // Arrange
            var seats = new List<Seat>
            {
                new Seat { Price = 10 },
                new Seat { Price = 10 }
            };

            var user = new Account(
                "John",
                null,
                "Doe",
                "2000-01-01",
                new Dictionary<string, string>(),
                "john.doe@example.com",
                "password123",
                "1234567890",
                "CreditCard",
                true,
                new List<Reservation>(),
                new List<Reservation>()
            );

            // Act
            var result = Reservation.ApplyDiscount(seats, user);

            // Assert
            Assert.AreEqual(8.50, result[0].Price);
            Assert.AreEqual(8.50, result[1].Price);
        }

        [TestMethod]
        public void ApplyDiscount_Senior_ReturnsDiscountedSeats()
        {
            // Arrange
            var seats = new List<Seat>
            {
                new Seat { Price = 10 },
                new Seat { Price = 10 }
            };

            var user = new Account(
                "Jane",
                null,
                "Doe",
                "1950-01-01",
                new Dictionary<string, string>(),
                "jane.doe@example.com",
                "password123",
                "1234567890",
                "CreditCard",
                false,
                new List<Reservation>(),
                new List<Reservation>()
            );

            // Act
            var result = Reservation.ApplyDiscount(seats, user);

            // Assert
            Assert.AreEqual(8.50, result[0].Price);
            Assert.AreEqual(8.50, result[1].Price);
        }

        [TestMethod]
        public void ApplyDiscount_NonStudentNonSenior_ReturnsNonDiscountedSeats()
        {
            // Arrange
            var seats = new List<Seat>
            {
                new Seat { Price = 10 },
                new Seat { Price = 10 }
            };

            var user = new Account(
                "Jack",
                null,
                "Doe",
                "1980-01-01",
                new Dictionary<string, string>(),
                "jack.doe@example.com",
                "password123",
                "1234567890",
                "CreditCard",
                false,
                new List<Reservation>(),
                new List<Reservation>()
            );

            // Act
            var result = Reservation.ApplyDiscount(seats, user);

            // Assert
            Assert.AreEqual(10, result[0].Price);
            Assert.AreEqual(10, result[1].Price);
        }
    }
}
