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
            // Print de absolute paths voor debugging
            string absoluteJsonPath = Path.GetFullPath(jsonFilePath);
            string absoluteCsvPath = Path.GetFullPath(csvFilePath);

            Console.WriteLine($"Checking JSON Path: {absoluteJsonPath}");
            Console.WriteLine($"Checking CSV Path: {absoluteCsvPath}");

            // Assert
            bool jsonExists = File.Exists(jsonFilePath);
            bool csvExists = File.Exists(csvFilePath);

            Assert.IsTrue(jsonExists || csvExists, "Movies.json of Movies.csv bestaat niet in de map MegaBios.");
        }

        [TestMethod]
        public void CheckIfMoviesJsonOrCsvExistsAndIsNotEmpty()
        {
            // Print de absolute paths voor debugging
            string absoluteJsonPath = Path.GetFullPath(jsonFilePath);
            string absoluteCsvPath = Path.GetFullPath(csvFilePath);

            Console.WriteLine($"Checking JSON Path: {absoluteJsonPath}");
            Console.WriteLine($"Checking CSV Path: {absoluteCsvPath}");

            // Controleer of een van beide bestanden bestaat
            bool jsonExists = File.Exists(jsonFilePath);
            bool csvExists = File.Exists(csvFilePath);

            // Bevestig dat een van de bestanden bestaat
            Assert.IsTrue(jsonExists || csvExists, "Movies.json of Movies.csv bestaat niet in de map MegaBios.");

            // Controleer of het bestand niet leeg is
            if (jsonExists)
            {
                Assert.IsTrue(new FileInfo(jsonFilePath).Length > 0, "Movies.json bestaat maar is leeg.");
            }

            if (csvExists)
            {
                Assert.IsTrue(new FileInfo(csvFilePath).Length > 0, "Movies.csv bestaat maar is leeg.");
            }
        }
    }
}
