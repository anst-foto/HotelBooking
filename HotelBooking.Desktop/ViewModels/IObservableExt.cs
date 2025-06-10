using System;
using System.Reactive.Linq;
using System.Threading.Tasks;

namespace HotelBooking.Desktop.ViewModels;

public static class IObservableExt
{
    public static IDisposable SubscribeAsync<T>(this IObservable<T> source, Func<T, Task> func) =>
        source.Select(o => Observable.FromAsync(_ => func(o)))
            .Concat()
            .Subscribe();
}