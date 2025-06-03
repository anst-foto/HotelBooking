namespace HotelBooking.Models;

/// <summary>
/// Комната
/// </summary>
public record Room : IModel
{
    /// <summary>
    /// Идентификатор
    /// </summary>
    public int? Id { get; init; }
    
    /// <summary>
    /// Номер
    /// </summary>
    public required string Number { get; init; }
    
    /// <summary>
    /// Признак удаления
    /// </summary>
    public bool IsDeleted { get; init; } = false;
}