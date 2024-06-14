using System.Text.Json.Serialization;

namespace MegaBios
{
    public abstract class User {
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

        protected User(string voornaam, string tussenvoegsel, string achternaam, string email, List<Reservation> reservations) {
            Voornaam = voornaam;
            Tussenvoegsel = tussenvoegsel;
            Achternaam = achternaam;
            Email = email;
            Reservations = reservations;
        }
    }

    public class Guest : User
    {
        [JsonConstructor]
        public Guest(string voornaam, string tussenvoegsel, string achternaam, string email, List<Reservation> reservations) : base(voornaam, tussenvoegsel, achternaam, email, reservations)
        {}

        public static Guest CreateGuest()
        {
            if (Environment.GetEnvironmentVariable("IS_TEST_ENVIRONMENT") != "true")
            {
                Console.Clear();
            }

            Console.WriteLine("\nVoer je gegevens in");
            Console.WriteLine("Druk op enter om te bevestigen");
            Console.WriteLine("--------------------");

            Console.Write("Voer voornaam in: ");
            string voornaam = Console.ReadLine()!;

            Console.Write("Voer tussenvoegsel in (als u een tussenvoegsel heeft): ");
            string tussenvoegsel = Console.ReadLine()!;

            Console.Write("Voer achternaam in: ");
            string achternaam = Console.ReadLine()!;

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

            return null!;
        }

        public static bool Equals(Guest g1, Guest g2) {
            if (g1 == null! || g2 == null!) {
                return g1! == g2!;
            }
            return g1 == g2;
        }

        public static bool operator ==(Guest g1, Guest g2)
        {
            if (g1 is null || g2 is null)
            {
                return g1 is null && g2 is null;
            }

            return g1.Email.Equals(g2.Email) && g1.Voornaam.Equals(g2.Voornaam) && g1.Achternaam.Equals(g2.Achternaam);
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

    // public class Account : Guest
    // {
    //     [JsonPropertyName("voornaam")]
    //     public string Voornaam { get; set; }

    //     [JsonPropertyName("tussenvoegsel")]
    //     public string Tussenvoegsel { get; set; }

    //     [JsonPropertyName("achternaam")]
    //     public string Achternaam { get; set; }

    //     [JsonPropertyName("geboorte_datum")]
    //     public string GeboorteDatum { get; set; }

    //     [JsonPropertyName("adres")]
    //     public Dictionary<string, string> Adres { get; set; }

    //     [JsonPropertyName("email")]
    //     public string Email { get; set; }

    //     [JsonPropertyName("wachtwoord")]
    //     public string Wachtwoord { get; set; }

    //     [JsonPropertyName("telefoonnummer")]
    //     public string TelefoonNr { get; set; }

    //     [JsonPropertyName("voorkeur_betaalwijze")]
    //     public string Voorkeur_Betaalwijze { get; set; }

    //     [JsonPropertyName("is_student")]
    //     public bool IsStudent { get; set; }

    //     [JsonPropertyName("reservations")]
    //     public List<Reservation> Reservations { get; set; }

    //     [JsonPropertyName("history")]
    //     public List<Reservation> History { get; set; }

    //     [JsonConstructor]
    //     public Account(string voornaam,
    //                         string tussenvoegsel,
    //                         string achternaam,
    //                         string geboorteDatum,
    //                         Dictionary<string, string> adres,
    //                         string email,
    //                         string wachtwoord,
    //                         string telefoonNr,
    //                         string voorkeur_Betaalwijze,
    //                         bool isStudent,
    //                         List<Reservation> reservations,
    //                         List<Reservation> history = null)
    //     {
    //         Voornaam = voornaam;
    //         Tussenvoegsel = tussenvoegsel;
    //         Achternaam = achternaam;
    //         GeboorteDatum = geboorteDatum;
    //         Adres = adres;
    //         Email = email;
    //         Wachtwoord = wachtwoord;
    //         TelefoonNr = telefoonNr;
    //         Voorkeur_Betaalwijze = voorkeur_Betaalwijze;
    //         IsStudent = isStudent;
    //         Reservations = reservations;
    //         if (history != null)
    //         {
    //             History = history;
    //         }
    //         else
    //         {
    //             History = new();
    //         }
    //     }

    public class Account : User
    {

        [JsonPropertyName("geboorte_datum")]
        public string GeboorteDatum { get; set; }

        [JsonPropertyName("adres")]
        public Dictionary<string, string> Adres { get; set; }

        [JsonPropertyName("wachtwoord")]
        public string Wachtwoord { get; set; }

        [JsonPropertyName("telefoonnummer")]
        public string TelefoonNr { get; set; }

        [JsonPropertyName("is_student")]
        public bool IsStudent { get; set; }

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
                            bool isStudent,
                            List<Reservation> reservations,
                            List<Reservation> history = null) : base(voornaam, tussenvoegsel, achternaam, email, reservations)
        {

            GeboorteDatum = geboorteDatum;
            Adres = adres;
            Wachtwoord = wachtwoord;
            TelefoonNr = telefoonNr;
            IsStudent = isStudent;

            if (history != null)
            {
                History = history;
            }
            else
            {
                History = new();
            }
        }

        public static bool Equals(Account a1, Account a2) {
            if (a1 == null! || a2 == null!) {
                return a1! == a2!;
            }
            return a1.Email.Equals(a2.Email) && a1.Wachtwoord.Equals(a2.Wachtwoord);
        }

        public static bool operator ==(Account a1, Account a2)
        {
            if (a1 is null || a2 is null)
            {
                return a1 is null && a2 is null;
            }
            return a1.Email.Equals(a2.Email) && a1.Wachtwoord.Equals(a2.Wachtwoord);
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
            return Voornaam == "admin" && Achternaam == "admin" && Email == "admin" && Wachtwoord == "admin";
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
