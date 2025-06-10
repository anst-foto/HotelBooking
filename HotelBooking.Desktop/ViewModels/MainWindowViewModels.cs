using System;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Net.Http.Json;
using System.Reactive;
using System.Threading.Tasks;
using System.Windows;
using HotelBooking.Models;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace HotelBooking.Desktop.ViewModels;

public class MainWindowViewModels : ReactiveObject
{
    private readonly HttpClient _httpClient;
    
    public ObservableCollection<Room> Rooms { get; set; } = [];
    [Reactive] public Room? SelectedRoom { get; set; }

    [Reactive] public int? Id { get; set; }
    [Reactive] public string? Number { get; set; }
    [Reactive] public bool? IsDeleted { get; set; }

    public ReactiveCommand<Unit, Unit> LoadCommand { get; }

    public MainWindowViewModels()
    {
        _httpClient = App.HttpClient;
        
        LoadCommand = ReactiveCommand.CreateFromTask(Load);

        this.WhenAnyValue(vm => vm.SelectedRoom)
            .WhereNotNull()
            .SubscribeAsync(GetById);
    }

    private async Task Load()
    {
        try
        {
            var url = "http://localhost:5117/api/v1/rooms";
            var rooms = _httpClient.GetFromJsonAsAsyncEnumerable<Room>(url);

            Rooms.Clear();
            await foreach (var room in rooms)
            {
                Rooms.Add(room);
            }
        }
        catch(Exception e)
        {
            MessageBox.Show(e.Message,"Error");
        }
    }

    private async Task GetById(Room selectedRoom)
    {
        try
        {
            var id = selectedRoom?.Id;
            var room = await _httpClient.GetFromJsonAsync<Room>($"http://localhost:5117/api/v1/rooms/{id}");
        
            Id = room?.Id;
            Number = room?.Number;
            IsDeleted = room?.IsDeleted;
        }
        catch (Exception e)
        {
            MessageBox.Show(e.Message, "Error");
        }
    }
}