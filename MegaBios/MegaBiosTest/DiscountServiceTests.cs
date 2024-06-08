using Microsoft.VisualStudio.TestTools.UnitTesting;
using MegaBios;
using System;
using System.Collections.Generic;

namespace MegaBiosTest.Services
{
    [TestClass]
    public class DiscountServiceTests
    {
        [TestMethod]
        public void ApplyDiscount_Student_ReturnsDiscountedSeats()
        {
            // Arrange
            var seats = new List<Seat>
            {
                new Seat { Price = 100 },
                new Seat { Price = 100 }
            };
            var user = new Account
            {
                Voornaam = "John",
                Achternaam = "Doe",
                GeboorteDatum = "2000-01-01",
                IsStudent = true
            };

            // Act
            var result = DiscountService.ApplyDiscount(seats, user);

            // Assert
            Assert.AreEqual(85, result[0].Price);
            Assert.AreEqual(85, result[1].Price);
        }

        [TestMethod]
        public void ApplyDiscount_Senior_ReturnsDiscountedSeats()
        {
            // Arrange
            var seats = new List<Seat>
            {
                new Seat { Price = 100 },
                new Seat { Price = 100 }
            };
            var user = new Account
            {
                Voornaam = "John",
                Achternaam = "Doe",
                GeboorteDatum = "1950-01-01",
                IsStudent = false
            };

            // Act
            var result = DiscountService.ApplyDiscount(seats, user);

            // Assert
            Assert.AreEqual(85, result[0].Price);
            Assert.AreEqual(85, result[1].Price);
        }

        [TestMethod]
        public void ApplyDiscount_NonStudentNonSenior_ReturnsNonDiscountedSeats()
        {
            // Arrange
            var seats = new List<Seat>
            {
                new Seat { Price = 100 },
                new Seat { Price = 100 }
            };
            var user = new Account
            {
                Voornaam = "John",
                Achternaam = "Doe",
                GeboorteDatum = "1980-01-01",
                IsStudent = false
            };

            // Act
            var result = DiscountService.ApplyDiscount(seats, user);

            // Assert
            Assert.AreEqual(100, result[0].Price);
            Assert.AreEqual(100, result[1].Price);
        }

        [TestMethod]
        public void ApplyDiscount_StudentAndSenior_ReturnsDiscountedSeats()
        {
            // Arrange
            var seats = new List<Seat>
            {
                new Seat { Price = 100 },
                new Seat { Price = 100 }
            };
            var user = new Account
            {
                Voornaam = "John",
                Achternaam = "Doe",
                GeboorteDatum = "1950-01-01",
                IsStudent = true
            };

            // Act
            var result = DiscountService.ApplyDiscount(seats, user);

            // Assert
            Assert.AreEqual(85, result[0].Price);
            Assert.AreEqual(85, result[1].Price);
        }
    }
}
