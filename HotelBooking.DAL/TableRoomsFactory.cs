using System;
using System.IO;
using HotelBooking.Models;
using Microsoft.Extensions.Configuration;

namespace HotelBooking.DAL;

public class TableRoomsFactory : ITableFactory<TableRooms, Room>
{
    public string? ConnectionString { get; private set; }
    public TableRooms Create(
        string fileName = "appsettings.json", 
        string connectionStringName = "DefaultConnection")
    {
        if (string.IsNullOrWhiteSpace(fileName)) 
            throw new ArgumentNullException(nameof(fileName));
        if (string.IsNullOrWhiteSpace(connectionStringName)) 
            throw new ArgumentNullException(nameof(connectionStringName));

        
        try
        {
            ConnectionString = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile(fileName)
                .Build()
                .GetConnectionString(connectionStringName);
        }
        catch (Exception e)
        {
            throw new ConfigurationException("Ошибка с конфигурацией", e);
        }
        
        if (string.IsNullOrEmpty(ConnectionString)) 
            throw new ConfigurationException("Строка подключения пуста");
        
        return new TableRooms(ConnectionString);
    }

    /*public static TableRooms Create()
    {
        var factory = new TableRoomsFactory();
        return factory.Create();
    }*/
}