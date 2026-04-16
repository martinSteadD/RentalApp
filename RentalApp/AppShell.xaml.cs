using RentalApp.ViewModels;
using RentalApp.Views;

namespace RentalApp;

public partial class AppShell : Shell
{
    public AppShell(AppShellViewModel viewModel)
    {   
        InitializeComponent();
        BindingContext = viewModel;

        // Register navigation routes using nameof() so DI resolves pages correctly
        Routing.RegisterRoute(nameof(ProfilePage), typeof(ProfilePage));
        Routing.RegisterRoute(nameof(TempPage), typeof(TempPage));
        Routing.RegisterRoute(nameof(BrowseItemsPage), typeof(BrowseItemsPage));
        Routing.RegisterRoute(nameof(UserListPage), typeof(UserListPage));
        Routing.RegisterRoute(nameof(UserDetailPage), typeof(UserDetailPage));
        Routing.RegisterRoute(nameof(ItemDetailPage), typeof(ItemDetailPage));
        Routing.RegisterRoute(nameof(MyItemsPage), typeof(MyItemsPage));
        Routing.RegisterRoute(nameof(CreateItemPage), typeof(CreateItemPage));
    }
}
