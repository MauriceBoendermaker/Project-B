using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using System.Threading;
using MegaBios;
using System.Linq.Expressions;

namespace MegaBiosTest.Services
{
    [TestClass]
    public class CinemaRoomGeneratorTests
    {
        private string basePath = "../../../";
        private string filePattern = "Room*.json";
        private string testRedirecionPath = "../../../../MegaBios/obj/Debug/net8.0/";

        [TestInitialize]
        public void TestInitialize()
        {
            // Stel de omgevingsvariabele in voor de testomgeving
            Environment.SetEnvironmentVariable("IS_TEST_ENVIRONMENT", "true");

            // Verwijder alle bestaande testbestanden
            var existingFiles = Directory.GetFiles(testRedirecionPath + basePath, filePattern);
            foreach (var file in existingFiles)
            {
                File.Delete(file);
            }
        }

        [TestCleanup]
        public void TestCleanup()
        {
            // Verwijder de omgevingsvariabele na de test
            Environment.SetEnvironmentVariable("IS_TEST_ENVIRONMENT", null);

            // Verwijder alle testbestanden
            var existingFiles = Directory.GetFiles(testRedirecionPath + basePath, filePattern);
            foreach (var file in existingFiles)
            {
                File.Delete(file);
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

        private int GetCurrentRoomCount()
        {
            var existingFiles = Directory.GetFiles(testRedirecionPath + basePath, filePattern);
            return existingFiles.Length;
        }

        [TestMethod]
        public void GenerateShowingData_ShouldCreateNextRoom()
        {
            // Arrange
            var generator = new CinemaRoomGenerator();
            var initialRoomCount = GetCurrentRoomCount();
            var nextRoomNumber = initialRoomCount + 1;
            var nextRoomFilePath = Path.Combine(testRedirecionPath + basePath, $"Room{nextRoomNumber}.json");

            var input = new StringReader("1\n10\n10\n2024-01-01 10:00:00\n");
            Console.SetIn(input);

            // Act
            SuppressConsoleOutput(() =>
            {
                generator.GenerateShowingData();
            });

            // Assert
            Thread.Sleep(500); // Kleine vertraging om bestandssysteem te laten verwerken
            var fileExists = File.Exists(nextRoomFilePath);
            Assert.IsTrue(fileExists, $"Room{nextRoomNumber}.json is niet aangemaakt. in {nextRoomFilePath}");
        }

        [TestMethod]
        public void GenerateShowingData_ShouldIncreaseRoomCountByOne()
        {
            // Arrange
            var generator = new CinemaRoomGenerator();
            var initialRoomCount = GetCurrentRoomCount();

            var input = new StringReader("1\n10\n10\n2024-01-01 10:00:00\n");
            Console.SetIn(input);

            // Act
            SuppressConsoleOutput(() =>
            {
                generator.GenerateShowingData();
            });

            // Assert
            Thread.Sleep(500); // Kleine vertraging om bestandssysteem te laten verwerken
            var newRoomCount = GetCurrentRoomCount();
            Assert.AreEqual(initialRoomCount + 1, newRoomCount, "Het aantal zalen is niet met 1 toegenomen.");
        }
    }
}
