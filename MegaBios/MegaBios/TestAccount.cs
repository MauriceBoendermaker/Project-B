using System.Text.Json;
using System.Text.Json.Serialization;

namespace MegaBios
{
    public class User
    {
        [JsonPropertyName("voornaam")]
        public string Voornaam { get; set; }

        [JsonPropertyName("tussenvoegsel")]
        public string Tussenvoegsel { get; set; }

        [JsonPropertyName("achternaam")]
        public string Achternaam { get; set; }

        [JsonPropertyName("email")]
        public string Email { get; set; }

        [JsonPropertyName("is_student")]
        public bool IsStudent { get; set; }

        [JsonPropertyName("history")]
        public List<ReservationHistory> History { get; set; }

        public User(string voornaam, string tussenvoegsel, string achternaam, string email, bool isStudent)
        {
            Voornaam = voornaam;
            Tussenvoegsel = tussenvoegsel;
            Achternaam = achternaam;
            Email = email;
            IsStudent = isStudent;
            History = new List<ReservationHistory>();
        }
    }

    public class TestAccount : User
    {
        public string GeboorteDatum { get; set; }

        [JsonPropertyName("adres")]
        public Dictionary<string, string> Adres { get; set; }

        [JsonPropertyName("wachtwoord")]
        public string Wachtwoord { get; set; }

        [JsonPropertyName("telefoonnummer")]
        public string TelefoonNr { get; set; }

        [JsonPropertyName("voorkeur_betaalwijze")]
        public string Voorkeur_Betaalwijze { get; set; }

        public TestAccount(string voornaam,
                            string tussenvoegsel,
                            string achternaam,
                            string geboorteDatum,
                            Dictionary<string, string> adres,
                            string email,
                            string wachtwoord,
                            string telefoonNr,
                            string voorkeur_Betaalwijze,
                            bool isStudent,
                            List<ReservationHistory> history) : base(voornaam, tussenvoegsel, achternaam, email, isStudent)

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
            History = new List<ReservationHistory>();
        }

        public static bool operator ==(TestAccount t1, TestAccount t2)
        {
            if (t1 is null || t2 is null)
            {
                return (t1 is null && t2 is null);
            }
            return t1.Email.Equals(t2.Email) && t1.Wachtwoord.Equals(t2.Wachtwoord);
        }

        public static bool operator !=(TestAccount t1, TestAccount t2)
        {
            return !(t1 == t2);
        }
    }
}