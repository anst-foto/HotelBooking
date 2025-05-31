using HotelBooking.Models;

namespace HotelBooking.DAL;

public interface ITableFactory<out T, T1> 
    where T : ICrudAsync<T1> 
    where T1 : IModel
{
    public string? ConnectionString { get; }
    public T Create(string fileName, string connectionStringName);
}