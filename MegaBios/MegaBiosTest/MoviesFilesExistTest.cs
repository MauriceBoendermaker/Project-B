using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;

namespace MegaBiosTest.Services
{
    [TestClass]
    public class MoviesFilesExistTest
    {
        private string jsonFilePath = "../../../../MegaBios/Movies.json";
        private string csvFilePath = "../../../../MegaBios/Movies.csv";

        [TestMethod]
        public void CheckIfMoviesJsonOrCsvExists()
        {
            // Print the absolute paths for debugging
            string absoluteJsonPath = Path.GetFullPath(jsonFilePath);
            string absoluteCsvPath = Path.GetFullPath(csvFilePath);

            Console.WriteLine($"Checking JSON Path: {absoluteJsonPath}");
            Console.WriteLine($"Checking CSV Path: {absoluteCsvPath}");

            // Assert
            bool jsonExists = File.Exists(jsonFilePath);
            bool csvExists = File.Exists(csvFilePath);

            Assert.IsTrue(jsonExists || csvExists, "Movies.json of Movies.csv bestaat niet in de map MegaBios.");
        }
    }
}
