namespace MegaBios
{
    public class CinemaRoomGenerator {
        public void GenerationMenu() {
            int userChoice;

            Console.Clear();

            List<string> menuOptions = new() {"Genereer nieuwe zalen", "Werk bestaande zaal bij", "Reset alle seatings"};
            System.Console.WriteLine("Do you want to generate rooms or edit a room?");


            userChoice = MenuFunctions.Menu(menuOptions);

            switch (userChoice) {
                case -1: 
                    return;
                case 0:
                    GenerateShowingData();
                    break;
                case 1: 
                    EditRoom();
                    break;
                case 2:
                    ResetAllSeatings();
                    break;
                default:
                    System.Console.WriteLine("Invalid input");
                    break;
            }
        }

        public void EditRoom() {
            //
        }

        public void ResetAllSeatings() { 
            for (int i = 1; File.Exists($"../../../Room{i}.json"); i++) {
                List<RoomShowing> roomshowings = JsonFunctions.LoadRoomShowings($"../../../Room{i}.json");
                for(int j = 0; j < roomshowings.Count; j++) {

                    roomshowings[i].Seating = ResetSeating(roomshowings[i].Seating);
                }

                JsonFunctions.WriteToJson($"../../../Room{i}.json", roomshowings);
            }
        }

        public List<List<Seat>> ResetSeating(List<List<Seat>> seating) {
            for (int i = 0; i < seating.Count; i++) {
                for (int j = 0; j < seating[i].Count; j++) {
                    seating[i][j].SeatTaken = false;
                }
            }

            return seating;
        }

        public void GenerateShowingData() {
            int numberOfRooms = 0; 
            for (int i = 1; File.Exists($"../../../Room{i}.json"); i++) {
                numberOfRooms = i;
            }

            int numberOfNewRooms = -1;
            List<RoomShowing> roomShowings;
            while (true) {
                System.Console.WriteLine("Enter the number of the amount of rooms you want to generate:");
                try {


                    numberOfNewRooms = Convert.ToInt32(Console.ReadLine());
                    break;
                }
                catch (Exception e) {
                    System.Console.WriteLine(e);
                }
                if (numberOfRooms > 0) {
                    break;
                }
            }
            // Get the seating
            for (int i = 1 + numberOfRooms; i < numberOfNewRooms + numberOfRooms + 1; i++) {

                string roomName = $"Room {i}";
                bool inMaintenance = false;
                List<List<Seat>>? seating;
                while(true) {
                    try {
                        System.Console.WriteLine("Hoe lang moet de zaal zijn? (Max 30)");

                        int roomHeight = Convert.ToInt32(Console.ReadLine());
                        if (roomHeight > 30) {
                            System.Console.WriteLine("Kamerlengte te groot, verzet naar 30");

                            roomHeight = 30;
                        }
                        else if (roomHeight <= 0) {
                            System.Console.WriteLine("Kamerlengte te klein, verzet naar 1");
                            roomHeight = 1;
                        }
                        System.Console.WriteLine("Hoe breed moet de zaal zijn? Max 50");


                        int roomWidth = Convert.ToInt32(Console.ReadLine());
                        if (roomWidth > 30) {
                            System.Console.WriteLine("Kamerbreedte te groot, verzet naar 30");

                            roomWidth = 30;
                        }
                        else if (roomWidth <= 0) {
                            System.Console.WriteLine("Kamerbreedte te klein, verzet naar 1");
                            roomWidth = 1;
                        }

                        seating = JsonFunctions.GenerateSeating(roomHeight, roomWidth);
                        roomShowings = GenerateRoomShowings(roomName, inMaintenance, seating);

                        JsonFunctions.WriteToJson($"../../../Room{i}.json", roomShowings);

                        break;
                    }
                    catch (Exception e) {
                        System.Console.WriteLine(e);
                        System.Console.WriteLine("Voer alsjeblieft een nummer in");
                    }
                }
            }
        }
        public List<RoomShowing> GenerateRoomShowings(string roomName, bool inMaintenance, List<List<Seat>> seating) {

            List<RoomShowing> roomShowings = new();
            List<Movie> movies = JsonFunctions.LoadMovies("../../../Movies.json");
            DateTime generationStartTime;
            while(true) {

                Console.Clear();
                System.Console.WriteLine("Enter the start date and time for the first showing. The showings will generate until a week later.\nFormat: YYYY-MM-DD hh-mm-ss");
                try {

                    generationStartTime = Convert.ToDateTime(Console.ReadLine());
                    break;
                }
                catch (Exception e) {
                    System.Console.WriteLine(e);
                }
            }
            System.Console.WriteLine("test");


            DateTime generationCurrentTime = generationStartTime;
            TimeSpan generationTs = generationCurrentTime - generationStartTime;
            Random rand = new Random();
            while (generationTs.Days < 7) {

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