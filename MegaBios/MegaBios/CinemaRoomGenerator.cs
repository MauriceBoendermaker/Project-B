namespace MegaBios
{
    public class CinemaRoomGenerator
    {
        public void GenerationMenu()
        {
            int userChoice;

            Console.Clear();

            List<string> menuOptions = new() { "Genereer nieuwe zalen", "Reset alle seatings" };

            Console.WriteLine("Wilt u zalen genereren of een zaal bewerken?");

            userChoice = MenuFunctions.Menu(menuOptions);

            switch (userChoice)
            {
                case -1:
                    return;
                case 0:
                    GenerateShowingData();
                    break;
                case 1:
                    ResetAllSeatings();
                    break;
                default:
                    Console.WriteLine("Ongeldige invoer");
                    break;
            }
        }

        public void ResetAllSeatings()
        {
            for (int i = 1; File.Exists($"../../../Room{i}.json"); i++)
            {
                List<RoomShowing> roomshowings = JsonFunctions.LoadRoomShowings($"../../../Room{i}.json");

                for (int j = 0; j < roomshowings.Count; j++)
                {
                    roomshowings[i].Seating = ResetSeating(roomshowings[i].Seating);
                }

                JsonFunctions.WriteToJson($"../../../Room{i}.json", roomshowings);
            }
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

        public void GenerateShowingData()
        {
            int numberOfRooms = 0;

            // Bereken het aantal bestaande zalen
            if (Environment.GetEnvironmentVariable("IS_TEST_ENVIRONMENT") != "true")
                {
                    for (int i = 1; File.Exists($"../../../../MegaBios/obj/Debug/net8.0/../../../Room{i}.json"); i++)
                    {
                        numberOfRooms = i;
                    }
                }
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
                    catch
                    {
                        Console.WriteLine("Ongeldige invoer. Probeer het opnieuw.");
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

            Console.WriteLine("test");

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
