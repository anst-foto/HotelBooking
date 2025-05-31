using System;

namespace HotelBooking.DAL.Test;

public class TableRoomsFactoryTest
{
    private readonly TableRoomsFactory _factory = new();
    
    private const string FileName = "appsettings.json";
    private const string EmptyFileName = "";
    private const string BadFileName = "appsettings_bad.json";
    private const string NotFoundFileName = "app.json";
    
    private const string ConnectionStringName = "Test";
    private const string EmptyConnectionStringName = "";
    
    [Fact]
    public void EmptyFileName_NegativeTest()
    {
        Assert.Throws<ArgumentNullException>(() => _factory.Create(EmptyFileName));
    }
    
    [Fact]
    public void EmptyConnectionStringName_NegativeTest()
    {
        Assert.Throws<ArgumentNullException>(() => _factory.Create(FileName, EmptyConnectionStringName));
    }

    [Fact]
    public void NotFoundConfigurationFile_NegativeTest()
    {
        Assert.Throws<ConfigurationException>(() => _factory.Create(NotFoundFileName, ConnectionStringName));
    }
    
    [Fact]
    public void BadConfigurationFile_NegativeTest()
    {
        Assert.Throws<ConfigurationException>(() => _factory.Create(BadFileName, ConnectionStringName));
    }

    [Fact]
    public void Create_PositiveTest()
    {
        const string expected = "Server=127.0.0.1;Port=5432;Database=hotel_db;User Id=postgres;Password=1234;SearchPath=test;";
        
        _factory.Create(FileName, ConnectionStringName);
        var actual = _factory.ConnectionString;
        
        Assert.Equal(expected, actual);
    }
}