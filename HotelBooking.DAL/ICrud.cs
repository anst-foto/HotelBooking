using System.Collections.Generic;
using System.Threading.Tasks;
using HotelBooking.Models;

namespace HotelBooking.DAL;

/// <summary>
/// Контракт для CRUD операций
/// </summary>
/// <typeparam name="T">Тип сущности/модели</typeparam>
public interface ICrudAsync<T> 
    where T : IModel
{
    /// <summary>
    /// Получить все сущности
    /// </summary>
    /// <returns>Асинхронный итератор сущностей</returns>
    public IAsyncEnumerable<T> GetAllAsync();
    
    /// <summary>
    /// Получить сущность по id
    /// </summary>
    /// <param name="id">id сущности</param>
    /// <returns>Сущность или null</returns>
    public Task<T?> GetByIdAsync(int id);
    
    /// <summary>
    /// Создать сущность
    /// </summary>
    /// <param name="entity">Cущность</param>
    /// <returns>Созданная сущность</returns>
    public Task<T> CreateAsync(T entity);
    
    /// <summary>
    /// Обновить сущность
    /// </summary>
    /// <param name="entity">Cущность</param>
    /// <returns>Обновленная сущность</returns>
    public Task<T> UpdateAsync(T entity);
    
    /// <summary>
    /// Удалить сущность по id
    /// </summary>
    /// <param name="id">id сущности</param>
    public Task DeleteAsync(int id);
}