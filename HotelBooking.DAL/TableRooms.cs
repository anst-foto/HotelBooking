using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Dapper;
using HotelBooking.Models;
using Npgsql;

namespace HotelBooking.DAL;

public class TableRooms : ICrudAsync<Room>
{
    private readonly NpgsqlConnection _connection;
    
    public TableRooms(string connectionString)
    {
        if (string.IsNullOrWhiteSpace(connectionString)) 
            throw new ArgumentNullException("Строка подключения не может быть пустой!");
        
        _connection = new NpgsqlConnection(connectionString);
    }
    
    public IAsyncEnumerable<Room> GetAllAsync()
    {
        throw new System.NotImplementedException();
    }

    public async Task<Room> GetByIdAsync(uint id)
    {
        var sql = "SELECT * FROM table_rooms WHERE id = @id";
        return await _connection.QuerySingleAsync<Room>(sql, new { id });
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