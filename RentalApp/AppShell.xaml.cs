using RentalApp.ViewModels;
using RentalApp.Views;

namespace RentalApp;

public partial class AppShell : Shell
{
    public AppShell(AppShellViewModel viewModel)
    {   
        InitializeComponent();
        BindingContext = viewModel;

        // Register navigation routes
        Routing.RegisterRoute("ProfilePage", typeof(ProfilePage));
        Routing.RegisterRoute("TempPage", typeof(TempPage));
        Routing.RegisterRoute("UserListPage", typeof(UserListPage));
        Routing.RegisterRoute("UserDetailPage", typeof(UserDetailPage));
        Routing.RegisterRoute("BrowseItemsPage", typeof(BrowseItemsPage));
        Routing.RegisterRoute("ItemDetailPage", typeof(ItemDetailPage));
        Routing.RegisterRoute("MyItemsPage", typeof(MyItemsPage));
        Routing.RegisterRoute("CreateItemPage", typeof(CreateItemPage));


    }
}
