using System;

namespace HotelBooking.Models;

public class Reservation
{
    public int Id { get; set; }
    public Room Room { get; set; }
    public Client Client { get; set; }
    public DateTime ArrivalDate { get; set; }
    public DateTime DepartureDate { get; set; }
    public bool IsCancelled { get; set; }
}