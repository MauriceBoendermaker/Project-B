using System.Globalization;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace MegaBios
{
    public class CinemaRoomGenerator
    {
        public void GenerationMenu()
        {
            int userChoice;

            Console.Clear();

            List<string> menuOptions = new() { "Genereer nieuwe zalen", "Verwijder zaal", "Reset alle seatings", "Werk filmlijst bij"};

            Console.WriteLine("Wilt u zalen genereren of een zaal bewerken?");

            userChoice = MenuFunctions.Menu(menuOptions, null, true);

            switch (userChoice)
            {
                case -1:
                    return;
                case 0:
                    GenerateShowingData();
                    break;
                case 1:
                    RemoveRoom();
                    break;
                case 2:
                    ResetAllSeatings();
                    break;
                case 3: 
                    EditMovies();
                    break;
                default:
                    Console.WriteLine("Ongeldige invoer");
                    break;
            }
        }

        public void RemoveRoom() {
            List<string> menuOptions = new();
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("Welke zaal wilt u verwijderen?");
            for (int i = 1; File.Exists($"../../../Room{i}.json"); i++) {
                menuOptions.Append($"Room {i}");
            }
            int selectedOption = MenuFunctions.Menu(menuOptions, sb, true) + 1;
            Console.WriteLine("Weet u zeker dat u uw account wilt verwijderen? (ja/nee)\n");
            string confirmInput = Console.ReadLine()!;

            if (confirmInput == "ja")
            {
                File.Delete($"../../../Room{selectedOption}.json");
                return;
            }
            else if (confirmInput == "nee")
            {
                return;
            }
            else
            {
                Console.WriteLine("Invalide keuze. Probeer het alstublieft opnieuw.");
            }
            
        }

        public void EditMovies() {
            List<string> menuOptions = new() {"Voeg film toe", "Verwijder film"};
            int userChoice = MenuFunctions.Menu(menuOptions, null, true);

            switch (userChoice)
            {
                case -1:
                    return;
                case 0:
                    AddMovie();
                    break;
                case 1:
                    RemoveMovie();
                    break;
                default:
                    Console.WriteLine("Ongeldige invoer");
                    break;
            }
        }

        public void AddMovie() {
            List<Movie> movies = JsonFunctions.LoadMovies("../../../Movies.json");
            
            System.Console.WriteLine("Wat is de filmnaam?\n(Enter => bevestigen)");
            string moveieName = Console.ReadLine();
           
            System.Console.WriteLine("Wat is de beschrijving?\n(Enter => bevestigen)");
            string description = Console.ReadLine();
            
            System.Console.WriteLine("Wat is de filmlengte in minuten?\n(Enter => bevestigen)");
            int duration = Convert.ToInt32(Console.ReadLine());
            DateTime startDatum;
            while(true) {
                System.Console.WriteLine("Wat is de startdatum? (dd-mm-yy)\n(Enter => bevestigen)");
                string startDatumStr = Console.ReadLine();
                if (DateTime.TryParseExact(startDatumStr, "dd-MM-yyyy", null, System.Globalization.DateTimeStyles.None, out _)) {
                    startDatum = Convert.ToDateTime(DateTime.ParseExact(startDatumStr, "dd-MM-yyyy", CultureInfo.InvariantCulture));
                    break;
                }
            }
            DateTime eindDatum;
            while(true) {
                System.Console.WriteLine("Wat is de einddatum? (dd-mm-yy)\n(Enter => bevestigen)");
                string eindDatumStr = Console.ReadLine();
                if (DateTime.TryParseExact(eindDatumStr, "dd-MM-yyyy", null, System.Globalization.DateTimeStyles.None, out _)) {
                    eindDatum = Convert.ToDateTime(DateTime.ParseExact(eindDatumStr, "dd-MM-yyyy", CultureInfo.InvariantCulture));
                    break;
                }
            }
            
            
            System.Console.WriteLine("Wat is/zijn de genre(s). Verdeel elk genre met een \',\'\n(Enter => bevestigen)");
            string genres = Console.ReadLine();
            
            System.Console.WriteLine("Wat is de beoordeling? Voer in met 1 getal achter de komma\n(Enter => bevestigen)");
            double beoordeling = Convert.ToDouble(Console.ReadLine());
            movies.Add(new Movie {Title = moveieName, Description = description, Duration = duration, StartDate = startDatum, EndDate = eindDatum, Rating = beoordeling});
            JsonFunctions.WriteToJson("../../../Movies.json", movies);
            System.Console.WriteLine($"Film {moveieName} is verwijderd. Druk op een willekeurige knop om terug te keren");
            Console.ReadKey(true);
        }
        public void RemoveMovie() {
            List<Movie> movies = JsonFunctions.LoadMovies("../../../Movies.json");
            System.Console.WriteLine("Welke film wil je verwijderen?\n(Enter => bevestigen)");
            string moveieName = Console.ReadLine();
            
            for (int i = 0; i < movies.Count; i++ ){
                if (movies[i].Title == moveieName) {
                    movies.RemoveAt(i);
                }
            }
            JsonFunctions.WriteToJson("../../../Movies.json", movies);
            System.Console.WriteLine($"Film {moveieName} is verwijderd. Druk op een willekeurige knop om terug te keren");
            Console.ReadKey(true);
            
        }

        public List<List<Seat>> ResetSeating(List<List<Seat>> seating)
        {
            for (int i = 0; i < seating.Count; i++)
            {
                for (int j = 0; j < seating[i].Count; j++)
                {
                    seating[i][j].SeatTaken = false;
                }
            }

            return seating;
        }

        public void ResetAllSeatings()
        {
            for (int i = 1; File.Exists($"../../../Room{i}.json"); i++)
            {
                List<RoomShowing> roomshowings = JsonFunctions.LoadRoomShowings($"../../../Room{i}.json");
                
                for (int j = 0; j < roomshowings.Count; j++)
                {
                    roomshowings[j].Seating = ResetSeating(roomshowings[i].Seating);
                }

                JsonFunctions.WriteToJson($"../../../Room{i}.json", roomshowings);
            }
        }
        
        public void GenerateShowingData()
        {
            int numberOfRooms = 0;

            // Bereken het aantal bestaande zalen
            for (int i = 1; File.Exists($"../../../Room{i}.json"); i++)
            {
                numberOfRooms = i;
            }

            int numberOfNewRooms = -1;

            // Vraag de gebruiker om het aantal nieuwe zalen in te voeren
            while (true)
            {
                Console.WriteLine("Voer het nummer in van de aantal zalen die u wilt genereren:");

                try
                {
                    numberOfNewRooms = Convert.ToInt32(Console.ReadLine());
                    break;
                }
                catch 
                {
                    Console.WriteLine("Ongeldige invoer. Probeer het opnieuw.");
                }
            }

            // Genereer de nieuwe zalen
            for (int i = 1 + numberOfRooms; i < numberOfNewRooms + numberOfRooms + 1; i++)
            {
                string roomName = $"Room {i}";
                bool inMaintenance = false;
                List<List<Seat>> seating = null;

                while (true)
                {
                    try
                    {
                        Console.WriteLine("Hoe lang moet de zaal zijn? (Max. 30)");
                        int roomHeight = Convert.ToInt32(Console.ReadLine());

                        if (roomHeight > 30)
                        {
                            Console.WriteLine("Kamerlengte te groot, verzet naar 30");
                            roomHeight = 30;
                        }
                        else if (roomHeight <= 0)
                        {
                            Console.WriteLine("Kamerlengte te klein, verzet naar 1");
                            roomHeight = 1;
                        }

                        Console.WriteLine("Hoe breed moet de zaal zijn? Max 50");
                        int roomWidth = Convert.ToInt32(Console.ReadLine());

                        if (roomWidth > 50)
                        {
                            Console.WriteLine("Kamerbreedte te groot, verzet naar 50");
                            roomWidth = 50;
                        }
                        else if (roomWidth <= 0)
                        {
                            Console.WriteLine("Kamerbreedte te klein, verzet naar 1");
                            roomWidth = 1;
                        }

                        seating = JsonFunctions.GenerateSeating(roomHeight, roomWidth);
                        break;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Ongeldige invoer. Probeer het opnieuw.");
                        System.Console.WriteLine(ex);
                    }
                }

                var roomShowings = GenerateRoomShowings(roomName, inMaintenance, seating);
                JsonFunctions.WriteToJson($"../../../Room{i}.json", roomShowings);
                Console.WriteLine($"Room{i}.json is aangemaakt");
            }
        }

        public List<RoomShowing> GenerateRoomShowings(string roomName, bool inMaintenance, List<List<Seat>> seating)
        {
            List<RoomShowing> roomShowings = new();
            string moviesFilePath = Path.Combine("../../../Movies.json");
            List<Movie> movies = JsonFunctions.LoadMovies(moviesFilePath);
            DateTime generationStartTime;

            while (true)
            {
                if (Environment.GetEnvironmentVariable("IS_TEST_ENVIRONMENT") != "true")
                {
                    Console.Clear();
                }

                Console.WriteLine("Voer de startdatum en -tijd in voor de eerste vertoning. De vertoningen duren tot een week later.\nFormat: YYYY-MM-DD hh-mm-ss");

                try
                {
                    generationStartTime = Convert.ToDateTime(Console.ReadLine());
                    break;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            }
            DateTime generationCurrentTime = generationStartTime;
            TimeSpan generationTs = generationCurrentTime - generationStartTime;
            Random rand = new Random();

            while (generationTs.Days < 7)
            {
                Movie currentMovie = movies[rand.Next(movies.Count)];
                RoomShowing showingEntry = new RoomShowing(roomName, seating, currentMovie.Title, (DateTime)generationCurrentTime, inMaintenance);

                generationCurrentTime = generationCurrentTime.AddMinutes(currentMovie.Duration + 15);
                generationTs = generationCurrentTime - generationStartTime;

                roomShowings.Add(showingEntry);
            }

            return roomShowings;
        }
    }
}
