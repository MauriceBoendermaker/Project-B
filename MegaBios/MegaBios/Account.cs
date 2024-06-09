using System.Text.Json.Serialization;

namespace MegaBios
{
    public class Guest
    {
        [JsonPropertyName("voornaam")]
        public string Voornaam { get; set; }

        [JsonPropertyName("tussenvoegsel")]
        public string Tussenvoegsel { get; set; }

        [JsonPropertyName("achternaam")]
        public string Achternaam { get; set; }

        [JsonPropertyName("email")]
        public string Email { get; set; }

        [JsonPropertyName("reservations")]
        public List<Reservation> Reservations { get; set; }

        [JsonConstructor]
        public Guest(string voornaam, string tussenvoegsel, string achternaam, string email, List<Reservation> reservations)
        {
            Voornaam = voornaam;
            Tussenvoegsel = tussenvoegsel;
            Achternaam = achternaam;
            Email = email;
            Reservations = reservations;
        }

        public static Guest CreateGuest()
        {
            Console.WriteLine("\nCreëer gast account");
            Console.WriteLine("--------------------");

            Console.Write("Voer voornaam in: ");
            string voornaam = Console.ReadLine();

            Console.Write("Voer tussenvoegsel in (als u een tussenvoegsel heeft): ");
            string tussenvoegsel = Console.ReadLine();

            Console.Write("Voer achternaam in: ");
            string achternaam = Console.ReadLine();

            string email;

            while (true)
            {
                Console.Write("Voer email in: ");
                email = Console.ReadLine()!;

                if (CreateAccount.IsValidEmail(email))
                {
                    break;
                }
                else
                {
                    Console.WriteLine("Ongeldig emailadres. Probeer het opnieuw.");
                }
            }

            Guest newGuest = new Guest(voornaam, tussenvoegsel, achternaam, email, new());
            List<Guest> guests = JsonFunctions.LoadGuests("../../../guestreservations.json");

            if (!DoesGuestExist(guests, newGuest))
            {
                guests.Add(newGuest);
                JsonFunctions.WriteToJson("../../../guestreservations.json", guests);
            }
            else
            {
                return GetCorrespondingGuest(guests, newGuest);
            }

            return newGuest;
        }

        public static bool DoesGuestExist(List<Guest> guests, Guest guest)
        {
            foreach (Guest currentGuest in guests)
            {
                if (currentGuest == guest)
                {
                    return true;
                }
            }

            return false;
        }

        public static Guest GetCorrespondingGuest(List<Guest> guests, Guest guest)
        {
            foreach (Guest currentGuest in guests)
            {
                if (currentGuest == guest)
                {
                    return currentGuest;
                }
            }

            return null;
        }

        public static bool operator ==(Guest t1, Guest t2)
        {
            if (t1 is null || t2 is null)
            {
                return (t1 is null && t2 is null);
            }

            return t1.Email.Equals(t2.Email) && t1.Voornaam.Equals(t2.Voornaam) && t1.Achternaam.Equals(t2.Achternaam);
        }

        public static bool operator !=(Guest t1, Guest t2)
        {
            return !(t1 == t2);
        }

        public static void UpdateAccount(Guest guest)
        {
            List<Guest> guests = JsonFunctions.LoadGuests("../../../guestreservations.json");

            for (int i = 0; i < guests.Count; i++)
            {
                if (guests[i] == guest)
                {
                    guests[i] = guest;
                    JsonFunctions.WriteToJson("../../../guestreservations.json", guests);
                    break;
                }
            }
        }
    }

    public class Account
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

        [JsonPropertyName("reservations")]
        public List<Reservation> Reservations { get; set; }

        [JsonPropertyName("history")]
        public List<Reservation> History { get; set; }

        [JsonConstructor]
        public Account(string voornaam,
                            string tussenvoegsel,
                            string achternaam,
                            string geboorteDatum,
                            Dictionary<string, string> adres,
                            string email,
                            string wachtwoord,
                            string telefoonNr,
                            string voorkeur_Betaalwijze,
                            bool isStudent,
                            List<Reservation> reservations,
                            List<Reservation> history = null)
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
            Reservations = reservations;
            if (history != null)
            {
                History = history;
            }
            else
            {
                History = new();
            }
        }

        public static bool operator ==(Account t1, Account t2)
        {
            if (t1 is null || t2 is null)
            {
                return (t1 is null && t2 is null);
            }
            return t1.Email.Equals(t2.Email) && t1.Wachtwoord.Equals(t2.Wachtwoord);
        }

        public static bool operator !=(Account t1, Account t2)
        {
            return !(t1 == t2);
        }

        public static void UpdateAccount(Account account)
        {
            List<Account> customers = JsonFunctions.LoadCustomers("../../../customers.json");

            for (int i = 0; i < customers.Count; i++)
            {
                if (customers[i] == account)
                {
                    customers[i] = account;
                    JsonFunctions.WriteToJson("../../../customers.json", customers);
                    break;
                }
            }
        }

        public bool IsAdmin()
        {
            return Voornaam == "admin" && Achternaam == "admin" && Email == "admin@testmail.com" && Wachtwoord == "adminwachtwoord";
        }

        public Account ReloadAccount()
        {
            Program.jsonData = JsonFunctions.LoadCustomers("../../../customers.json");

            for (int i = 0; i < Program.jsonData.Count; i++)
            {
                if (Program.jsonData[i] == this)
                {
                    return Program.jsonData[i];
                }
            }

            return this;
        }
    }
}