using System.Net.Http;
using System.Windows;

namespace HotelBooking.Desktop;


public partial class App : Application
{
    public static HttpClient HttpClient = new HttpClient();
}