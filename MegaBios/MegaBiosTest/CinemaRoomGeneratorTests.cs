using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using System.Threading;
using MegaBios;

namespace MegaBiosTest.Services
{
    [TestClass]
    public class CinemaRoomGeneratorTests
    {
        private string filePath = "../../../Room1.json";

        [TestInitialize]
        public void TestInitialize()
        {
            // Stel de omgevingsvariabele in voor de testomgeving
            Environment.SetEnvironmentVariable("IS_TEST_ENVIRONMENT", "true");

            // Zorg ervoor dat het testbestand niet bestaat voor de test
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
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
        public void GenerateShowingData_ShouldCreateNewRoom()
        {
            // Arrange
            var generator = new CinemaRoomGenerator();
            var input = new StringReader("1\n10\n10\n2024-01-01 10:00:00\n");
            Console.SetIn(input);

            // Act
            SuppressConsoleOutput(() =>
            {
                generator.GenerateShowingData();
            });

            // Assert
            Thread.Sleep(500); // Kleine vertraging om bestandssysteem te laten verwerken
            var fileExists = File.Exists(filePath);
            Assert.IsTrue(fileExists, "Room1.json is niet aangemaakt.");
        }
    }
}
