using System;
using HotelBooking.DAL;
using HotelBooking.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddJsonFile("appsettings.json");

#if DEBUG
var connectionString = builder.Configuration.GetConnectionString("Test");
#elif RELEASE
var connectionString = builder.Configuration.GetConnectionString("Production");
#endif

var rooms = new TableRooms(connectionString);
builder.Services.AddSingleton<ICrudAsync<Room>>(rooms);

var app = builder.Build();


app.UseHttpsRedirection();

app.MapGet("/", () => Results.Ok("Hello World"));

const string apiPrefix = "/api/v1";

var apiRooms = app.MapGroup($"{apiPrefix}/rooms");
apiRooms.MapGet("/{id:int}", async (int id, ICrudAsync<Room> tableRooms) =>
{
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
apiRooms.MapGet("/", (ICrudAsync<Room> tableRooms) =>
{
    try
    {
        var rooms = tableRooms.GetAllAsync();
        return Results.Ok(rooms);
    }
    catch (Exception e)
    {
        return Results.Problem(e.Message, null, StatusCodes.Status422UnprocessableEntity);
    }
});

await app.RunAsync();