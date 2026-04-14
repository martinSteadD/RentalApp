using RentalApp.ViewModels;
using RentalApp.Helpers;

namespace RentalApp.Views;

public partial class SettingsPage : ContentPage
{
    public SettingsPage()
    {
        InitializeComponent();
        BindingContext = ServiceHelper.GetService<SettingsViewModel>();

        ToolbarItems.Add(new ToolbarItem
        {
            IconImageSource = "back.png",
            Command = new Command(async () => await Shell.Current.GoToAsync("//MainPage")),
            Priority = 0,
            Order = ToolbarItemOrder.Primary
        });
    }

}
