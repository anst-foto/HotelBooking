using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Dapper;
using HotelBooking.Models;
using Npgsql;

namespace HotelBooking.DAL;

/// <summary>
/// Таблица "Комнаты"
/// </summary>
public class TableRooms : ICrudAsync<Room>
{
    /// <summary>
    /// Подключение к БД
    /// </summary>
    private readonly NpgsqlConnection _connection;
    
    /// <summary>
    /// Конструктор
    /// </summary>
    /// <param name="connectionString">Строка подключения</param>
    /// <exception cref="ArgumentNullException">Строка подключения не может быть пустой!</exception>
    public TableRooms(string connectionString)
    {
        if (string.IsNullOrWhiteSpace(connectionString)) 
            throw new ArgumentNullException("Строка подключения не может быть пустой!");
        
        _connection = new NpgsqlConnection(connectionString);
    }
    
    /// <summary>
    /// Возвращает все записи из таблицы
    /// </summary>
    /// <returns>Асинхронный итератор все записей комнат</returns>
    public IAsyncEnumerable<Room> GetAllAsync()
    {
        throw new System.NotImplementedException();
    }

    /// <summary>
    /// Возвращает комнату из таблицы по id
    /// </summary>
    /// <param name="id">id комнаты</param>
    /// <returns>Найденная комната или null</returns>
    public async Task<Room?> GetByIdAsync(int id)
    {
        var sql = "SELECT * FROM table_rooms WHERE id = @id";
        return await _connection.QuerySingleOrDefaultAsync<Room>(sql, new { id });
    }

    public Task<Room> CreateAsync(Room entity)
    {
        throw new System.NotImplementedException();
    }

    public Task<Room> UpdateAsync(Room entity)
    {
        throw new System.NotImplementedException();
    }

    public Task DeleteAsync(int id)
    {
        throw new System.NotImplementedException();
    }
}