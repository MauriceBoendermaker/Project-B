using MegaBios;
using System.Text.Json;

namespace MegaBiosTest.Services
{
    [TestClass]
    public class CreateAccountIntegrationTests
    {
        private string filePath = "../../../customers.json";
        private string testRedirecionPath = "../../../../MegaBios/obj/Debug/net8.0/";

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
        public void CreateNewAccount_AccountAppearsInJsonFile()
        {
            // Arrange
            var jsonData = new List<Account>();
            if (File.Exists(testRedirecionPath + filePath))
            {
                var json = File.ReadAllText(filePath);
                jsonData = JsonSerializer.Deserialize<List<Account>>(json);
            }

            // Simuleer gebruikersinvoer
            var input = new StringReader("Bob\n\nVos\n2000-01-01\nstraat\n123\nDen Haag\n1234VV\njohn.doe@example.com\npassword123\npassword123\n1234567890\nCreditCard\nja\n");
            Console.SetIn(input);

            // Act
            SuppressConsoleOutput(() => {
                CreateAccount.CreateNewAccount(jsonData);
                JsonFunctions.WriteToJson(testRedirecionPath + filePath, jsonData);
            });

            // Assert
            var updatedJson = File.ReadAllText(testRedirecionPath + filePath);
            var updatedJsonData = JsonSerializer.Deserialize<List<Account>>(updatedJson);

            var createdAccount = updatedJsonData.Find(account => account.Email == "john.doe@example.com");
            Assert.IsNotNull(createdAccount, "Account should not be null.");
            Assert.AreEqual("Bob", createdAccount.Voornaam, "First name mismatch.");
            Assert.AreEqual("Vos", createdAccount.Achternaam, "Last name mismatch.");
            Assert.AreEqual("john.doe@example.com", createdAccount.Email, "Email mismatch.");
        }

        [TestMethod]
        public void DeleteAccount_AccountRemovedFromJsonFile()
        {
            // Arrange
            var jsonData = new List<Account>();

            if (File.Exists(filePath))
            {
                var json = File.ReadAllText(testRedirecionPath + filePath);
                jsonData = JsonSerializer.Deserialize<List<Account>>(json);
            }

            // Voeg een testaccount toe
            var testAccount = new Account("Alper", "", "Sonmez", "2003-04-10", new Dictionary<string, string>
            {
                { "straat", "Overijselsestraat" },
                { "huisnummer", "122b" },
                { "woonplaats", "Rotterdam" },
                { "postcode", "3074VJ" }
            }, "aziaatyt@gmail.com", "alper123", "0640518191", "IDeal", true, new List<Reservation>(), new List<Reservation>());

            jsonData.Add(testAccount);
            JsonFunctions.WriteToJson(testRedirecionPath + filePath, jsonData);

            // Act
            SuppressConsoleOutput(() => {
                DeleteAccount.RemoveAccount(jsonData, testAccount);
                JsonFunctions.WriteToJson(testRedirecionPath + filePath, jsonData);
            });

            // Assert
            var updatedJson = File.ReadAllText(filePath);
            var updatedJsonData = JsonSerializer.Deserialize<List<Account>>(updatedJson);
            var deletedAccount = updatedJsonData.Find(account => account.Email == "aziaatyt@gmail.com");
            Assert.IsNull(deletedAccount, "Account should be null.");
        }
    }
}
