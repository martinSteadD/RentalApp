using Microsoft.Maui.Controls;

namespace RentalApp.Helpers;

public static class ServiceHelper
{
    public static T GetService<T>() =>
        Current.GetService<T>();

    public static IServiceProvider Current =>
        IPlatformApplication.Current.Services;
}
