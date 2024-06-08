using Microsoft.VisualStudio.TestTools.UnitTesting;
using MegaBios;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace MegaBiosTest.Services
{
    [TestClass]
    public class CreateAccountIntegrationTests
    {
        private string filePath = "../../../customers.json";

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
            if (File.Exists(filePath))
            {
                var json = File.ReadAllText(filePath);
                jsonData = JsonSerializer.Deserialize<List<Account>>(json);
            }

            // Simuleer gebruikersinvoer
            var input = new StringReader("John\n\nDoe\n2000-01-01\nMain Street\n123\nCity\n12345\njohn.doe@example.com\npassword123\npassword123\n1234567890\nCreditCard\ntrue\n");
            Console.SetIn(input);

            // Act
            SuppressConsoleOutput(() => {
                CreateAccount.CreateNewAccount(jsonData);
                JsonFunctions.WriteToJson(filePath, jsonData);
            });

            // Assert
            var updatedJson = File.ReadAllText(filePath);
            var updatedJsonData = JsonSerializer.Deserialize<List<Account>>(updatedJson);

            var createdAccount = updatedJsonData.Find(account => account.Email == "john.doe@example.com");
            Assert.IsNotNull(createdAccount, "Account should not be null.");
            Assert.AreEqual("John", createdAccount.Voornaam, "First name mismatch.");
            Assert.AreEqual("Doe", createdAccount.Achternaam, "Last name mismatch.");
            Assert.AreEqual("john.doe@example.com", createdAccount.Email, "Email mismatch.");
        }
    }
}
