using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MegaBios
{
    internal class JsonFunctions
    {
        public static void WriteToJson(string filePath, List<TestAccount> data)
        {
            string jsonString = JsonSerializer.Serialize(data, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(filePath, jsonString);
        }

        public static TestAccount ConvertJsonToAccount(JsonElement user)
        {
            string voornaam = user.GetProperty("voornaam").GetString()!;
            string tussenvoegsel = user.GetProperty("tussenvoegsel").GetString()!;
            string achternaam = user.GetProperty("achternaam").GetString()!;
            string geboorteDatum = user.GetProperty("geboorte_datum").GetString()!;

            Dictionary<string, string> adres = new();
            JsonElement adresElement = user.GetProperty("adres")!;

            foreach (var property in adresElement.EnumerateObject())
            {
                adres.Add(property.Name, property.Value.GetString()!);
            }

            string email = user.GetProperty("email").GetString()!;
            string wachtwoord = user.GetProperty("wachtwoord").GetString()!;
            string telefoonNr = user.GetProperty("telefoonnummer").GetString()!;
            string betaalwijze = user.GetProperty("voorkeur_betaalwijze").GetString()!;
            bool is_student = user.GetProperty("is_student").GetBoolean()!;

            return new TestAccount(voornaam, tussenvoegsel, achternaam, geboorteDatum, adres, email, wachtwoord, telefoonNr, betaalwijze, is_student);
        }

        public static List<TestAccount> ConvertJsonToList(JsonElement root)
        {
            foreach (JsonElement user in root.EnumerateArray())
            {
                Program.jsonData.Add(ConvertJsonToAccount(user));
            }
            return Program.jsonData;
        }
    }
}
