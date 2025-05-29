namespace HotelBooking.Models;

public class Room : IModel
{
    public int Id { get; set; }
    public string Number { get; set; }
    public bool IsDeleted { get; set; }
}