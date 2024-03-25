using System.Text.Json.Serialization;

namespace MegaBios
{
    public class TestAccount
    {
        [JsonPropertyName("voornaam")]
        public string Voornaam { get; set; }

        [JsonPropertyName("tussenvoegsel")]
        public string Tussenvoegsel { get; set; }

        [JsonPropertyName("achternaam")]
        public string Achternaam { get; set; }
        [JsonPropertyName("geboorte_datum")]
        public string GeboorteDatum { get; set; }

        [JsonPropertyName("adres")]
        public Dictionary<string, string> Adres { get; set; }

        [JsonPropertyName("email")]
        public string Email { get; set; }

        [JsonPropertyName("wachtwoord")]
        public string Wachtwoord { get; set; }

        [JsonPropertyName("telefoonnummer")]
        public string TelefoonNr { get; set; }

        [JsonPropertyName("voorkeur_betaalwijze")]
        public string Voorkeur_Betaalwijze { get; set; }

        [JsonPropertyName("is_student")]
        public bool IsStudent { get; set; }

        public TestAccount(string voornaam, string tussenvoegsel, string achternaam, string geboorteDatum, Dictionary<string, string> adres,
                            string email, string wachtwoord, string telefoonNr, string voorkeur_Betaalwijze, bool isStudent)
        {
            Voornaam = voornaam;
            Tussenvoegsel = tussenvoegsel;
            Achternaam = achternaam;
            GeboorteDatum = geboorteDatum;
            Adres = adres;
            Email = email;
            Wachtwoord = wachtwoord;
            TelefoonNr = telefoonNr;
            Voorkeur_Betaalwijze = voorkeur_Betaalwijze;
            IsStudent = isStudent;
        }
    }
}