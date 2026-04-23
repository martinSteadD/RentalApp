using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using CommunityToolkit.Maui;
using RentalApp.ViewModels;
using RentalApp.Database.Data;
using RentalApp.Views;
using RentalApp.Services;

// Required for disabling Android ripple
using Microsoft.Maui.Controls.Handlers.Items;
using Android.Graphics.Drawables;

namespace RentalApp;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .UseMauiCommunityToolkit()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

        //  Register the CollectionView handler
        builder.ConfigureMauiHandlers(handlers =>
        {
            handlers.AddHandler<CollectionView, CollectionViewHandler>();
        });

        //  Disable Android ripple highlight
        CollectionViewHandler.Mapper.AppendToMapping("NoRipple", (handler, view) =>
        {
#if ANDROID
            handler.PlatformView.Foreground =
                new ColorDrawable(Android.Graphics.Color.Transparent);
#endif
        });

        // Register SQLite DbContext
        builder.Services.AddDbContext<AppDbContext>();

        // Local services
        builder.Services.AddSingleton<IAuthenticationService, AuthenticationService>();
        builder.Services.AddSingleton<INavigationService, NavigationService>();
        builder.Services.AddSingleton<IItemService, ItemService>();

        // Coursework API HttpClient
        builder.Services.AddSingleton<HttpClient>(sp =>
            new HttpClient
            {
                BaseAddress = new Uri("https://set09102-api.b-davison.workers.dev")
            });

        // Coursework API services
        builder.Services.AddSingleton<ApiClient>();
        builder.Services.AddSingleton<ApiAuthService>();
        builder.Services.AddSingleton<TokenStore>();

        // ViewModels + Views
        builder.Services.AddSingleton<AppShellViewModel>();
        builder.Services.AddSingleton<AppShell>();
        builder.Services.AddSingleton<App>();

        builder.Services.AddTransient<MainViewModel>();
        builder.Services.AddTransient<MainPage>();
        builder.Services.AddSingleton<LoginViewModel>();
        builder.Services.AddTransient<LoginPage>();
        builder.Services.AddSingleton<RegisterViewModel>();
        builder.Services.AddTransient<RegisterPage>();
        builder.Services.AddTransient<UserListViewModel>();
        builder.Services.AddTransient<UserListPage>();
        builder.Services.AddTransient<UserDetailPage>();
        builder.Services.AddTransient<UserDetailViewModel>();

        builder.Services.AddSingleton<ProfileViewModel>();
        builder.Services.AddTransient<ProfilePage>();
        builder.Services.AddTransient<SettingsViewModel>();
        builder.Services.AddTransient<SettingsPage>();

        // Browse Items
        builder.Services.AddTransient<BrowseItemsViewModel>();
        builder.Services.AddTransient<BrowseItemsPage>();

        builder.Services.AddTransient<ItemDetailPage>();
        builder.Services.AddTransient<ItemDetailViewModel>();
        builder.Services.AddTransient<MyItemsPage>();
        builder.Services.AddTransient<MyItemsViewModel>();
        builder.Services.AddTransient<CreateItemPage>();
        builder.Services.AddTransient<CreateItemViewModel>();
        builder.Services.AddSingleton<CategoryService>();

        builder.Services.AddSingleton<TempViewModel>();
        builder.Services.AddTransient<TempPage>();

#if DEBUG
        builder.Logging.AddDebug();
#endif

        // Build the app
        var app = builder.Build();

        // Ensure SQLite DB is created
        using (var scope = app.Services.CreateScope())
        {
            var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            db.Database.EnsureCreated();
        }

        return app;
    }
}
