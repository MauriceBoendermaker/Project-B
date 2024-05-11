using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MegaBios
{
    public class CinemaRoomGenerator {
        private string BaseFileName = "../../../CinemaRoom";

        public void GenerationMenu() {
            int userChoice = -1;
            int cursorPos = 0;
            while(true) {
                Console.Clear();
                List<string> menuOptions = new() {"Generate new rooms", "Edit existing room"};
                System.Console.WriteLine("Do you want to generate rooms or edit a room?");
                for (int i = 0; i < menuOptions.Count; i++) {
                    if (cursorPos == i) {
                        System.Console.WriteLine($"> {menuOptions[i]}");
                    }
                    else {
                        System.Console.WriteLine(menuOptions[i]);
                    }
                }
                ConsoleKeyInfo keyInfo = Console.ReadKey(true);
                if (keyInfo.Key == ConsoleKey.Enter) {
                    userChoice = cursorPos;
                    break;
                }
                else {
                    cursorPos = MenuFunctions.MoveCursor(cursorPos, keyInfo, menuOptions.Count);
                }
            }

            switch (userChoice) {
                case 0:
                    GenerateShowingData();
                    break;
                case 1: 
                    EditRoom();
                    break;
                default:
                    System.Console.WriteLine("Invalid input");
                    break;
            }
            

        }

        public void EditRoom() {
            //
        }

        public void GenerateShowingData() {
            MegaBiosData megaBiosData = JsonFunctions.LoadMegaBiosData("../../../MegaBiosData.json");
            int numberOfRooms = megaBiosData.AmountOfRooms;
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
                        System.Console.WriteLine("Hoe breed moet de zaal zijn?");
                        int roomWidth = Convert.ToInt32(Console.ReadLine());

                        System.Console.WriteLine("Hoe lang moet de zaal zijn?");
                        int roomHeight = Convert.ToInt32(Console.ReadLine());
                        seating = JsonFunctions.GenerateSeating(roomWidth, roomHeight);
                        roomShowings = GenerateRoomShowings(roomName, inMaintenance, seating);
                        JsonFunctions.WriteToJson($"../../../Room{i}.json", roomShowings);
                        break;
                    }
                    catch (Exception e) {
                        System.Console.WriteLine(e);
                        System.Console.WriteLine("Voer alsjeblieft een nummer in");
                    }
                }
                megaBiosData.AmountOfRooms += numberOfNewRooms;
                JsonFunctions.WriteToJson("../../../MegaBiosData.json", megaBiosData);
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