using System.Threading.Tasks;
using HotelBooking.Models;

namespace HotelBooking.DAL.Test;

public class TableRoomsTest
{
    private const string FileName = "appsettings.json";
    private const string ConnectionStringName = "Test";
    
    [Fact]
    public async Task GetByIdAsyncTest()
    {
        var expected = new Room()
        {
            Id = 1,
            Number = "101",
            IsDeleted = false
        };

        var factory = new TableRoomsFactory();

        var tableRooms = factory.Create(FileName, ConnectionStringName);
        var actual = await tableRooms.GetByIdAsync(1);
        
        Assert.Equal(expected, actual);
    }
}