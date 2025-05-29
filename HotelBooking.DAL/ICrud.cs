using System.Collections.Generic;
using System.Threading.Tasks;
using HotelBooking.Models;

namespace HotelBooking.DAL;

public interface ICrudAsync<T> 
    where T : IModel
{
    public IAsyncEnumerable<T> GetAllAsync();
    public Task<T> GetByIdAsync(int id);
    public Task<T> CreateAsync(T entity);
    public Task<T> UpdateAsync(T entity);
    public Task DeleteAsync(int id);
}