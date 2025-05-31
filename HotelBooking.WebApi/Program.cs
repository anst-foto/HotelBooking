using System;
using HotelBooking.DAL;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.UseHttpsRedirection();

app.MapGet("/", () => Results.Ok("Hello World"));

app.MapGet("/api/v1/rooms/{id:int}", async (uint id) =>
{
    var factory = new TableRoomsFactory();
    #if DEBUG
    var tableRooms = factory.Create("appsetings.json", "Test");
    #elif RELEASE
    var tableRooms = factory.Create("appsetings.json", "Production");
    #endif

    try
    {
        var room = await tableRooms.GetByIdAsync(id);
        return Results.Ok(room);
    }
    catch (Exception e)
    {
        return Results.Problem(e.Message, null, StatusCodes.Status422UnprocessableEntity);
    }
});

app.Run();
