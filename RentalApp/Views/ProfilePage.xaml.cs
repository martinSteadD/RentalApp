using RentalApp.ViewModels;

namespace RentalApp.Views;

public partial class ProfilePage : ContentPage
{
    public ProfilePage(ProfileViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;

        foreach (var item in ToolbarItems)
            item.BindingContext = vm;

    }
}
