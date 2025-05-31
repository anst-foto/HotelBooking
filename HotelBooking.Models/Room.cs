namespace HotelBooking.Models;

public record Room : IModel
{
    public uint? Id { get; init; }
    public required string Number { get; init; }
    public bool IsDeleted { get; init; } = false;
}