using System.Collections.Concurrent;

class Seat {
    public string SeatNumber {get;} 
    public string SeatType {get;}
    public bool SeatTaken {get;set;}

    public Seat(string seatNumber, string seatType, bool seatTaken) {
        SeatNumber = seatNumber;
        SeatType = seatType;
        SeatTaken = seatTaken;
    }
}