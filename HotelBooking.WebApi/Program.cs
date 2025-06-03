using System;
using HotelBooking.DAL;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.UseHttpsRedirection();

app.MapGet("/", () => Results.Ok("Hello World"));

const string apiPrefix = "/api/v1";

var apiRooms = app.MapGroup($"{apiPrefix}/rooms");

apiRooms.MapGet("/{id:int}", async (int id) =>
{
    var factory = new TableRoomsFactory();
   
    try
    {
#if DEBUG
        var tableRooms = factory.Create("appsettings.json", "Test");
#elif RELEASE
        var tableRooms = factory.Create("appsetings.json", "Production");
#endif
        
        var room = await tableRooms.GetByIdAsync(id);
        return Results.Ok(room);
    }
    catch (Exception e)
    {
        return Results.Problem(e.Message, null, StatusCodes.Status422UnprocessableEntity);
    }
});

await app.RunAsync();
