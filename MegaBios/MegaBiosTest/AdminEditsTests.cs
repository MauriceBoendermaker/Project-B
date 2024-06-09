using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using System.Collections.Generic;
using MegaBios;

namespace MegaBiosTest.Services
{
    [TestClass]
    public class AdminEditsTests
    {
        private string filePath = "../../../Room1.json";

        [TestInitialize]
        public void TestInitialize()
        {
            // Stel de omgevingsvariabele in voor de testomgeving
            Environment.SetEnvironmentVariable("IS_TEST_ENVIRONMENT", "true");

            // Maak een testbestand aan met een voorbeeldinhoud
            var roomShowings = new List<RoomShowing>
            {
                new RoomShowing
                {
                    RoomNumber = "Room 1",
                    Seating = JsonFunctions.GenerateSeating(5, 5),
                    Movie = "Test Movie",
                    ShowingTime = new DateTime(2024, 8, 25, 9, 0, 0),
                    InMaintenance = false
                },
                new RoomShowing
                {
                    RoomNumber = "Room 1",
                    Seating = JsonFunctions.GenerateSeating(5, 5),
                    Movie = "Test Movie",
                    ShowingTime = new DateTime(2024, 8, 25, 10, 0, 0),
                    InMaintenance = false
                }
            };

            JsonFunctions.WriteToJson(filePath, roomShowings);
        }

        [TestCleanup]
        public void TestCleanup()
        {
            // Verwijder de omgevingsvariabele na de test
            Environment.SetEnvironmentVariable("IS_TEST_ENVIRONMENT", null);

            // Verwijder het testbestand na de test
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
        }

        private void SuppressConsoleOutput(Action action)
        {
            var originalOut = Console.Out;
            var originalError = Console.Error;

            try
            {
                using (var writer = new StringWriter())
                {
                    Console.SetOut(writer);
                    Console.SetError(writer);
                    action();
                }
            }
            finally
            {
                Console.SetOut(originalOut);
                Console.SetError(originalError);
            }
        }

        [TestMethod]
        public void EditRoomSize_ShouldUpdateRoomDimensions()
        {
            // Arrange
            var input = new StringReader("1\n10\n20\n");
            Console.SetIn(input);

            // Act
            SuppressConsoleOutput(() =>
            {
                Program.EditRoomSize();
            });

            // Assert
            var updatedRoomShowings = JsonFunctions.LoadRoomShowings(filePath);
            var updatedSeating = updatedRoomShowings[0].Seating;

            Assert.AreEqual(10, updatedSeating.Count, "De hoogte van de zaal is niet correct bijgewerkt.");
            Assert.AreEqual(20, updatedSeating[0].Count, "De breedte van de zaal is niet correct bijgewerkt.");
        }

        [TestMethod]
        public void EditRoomSize_ShouldUpdateRoomDimensionsForAllShowings()
        {
            // Arrange
            var input = new StringReader("1\n10\n20\n");
            Console.SetIn(input);

            // Act
            SuppressConsoleOutput(() =>
            {
                Program.EditRoomSize();
            });

            // Assert
            var updatedRoomShowings = JsonFunctions.LoadRoomShowings(filePath);

            // Loop door de showings en check of de seating hetzelfde is
            foreach (var showing in updatedRoomShowings)
            {
                var updatedSeating = showing.Seating;

                Assert.AreEqual(10, updatedSeating.Count, "De hoogte van de zaal is niet correct bijgewerkt.");
                Assert.AreEqual(20, updatedSeating[0].Count, "De breedte van de zaal is niet correct bijgewerkt.");
            }
        }
    }
}
