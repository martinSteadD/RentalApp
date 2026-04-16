using RentalApp.ViewModels;

namespace RentalApp.Views;

public partial class BrowseItemsPage : ContentPage
{
    public BrowseItemsPage(BrowseItemsViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;

        Console.WriteLine("DEBUG: BrowseItemsPage BindingContext = " + BindingContext?.GetType().Name);
    }


    protected override async void OnAppearing()
    {
        base.OnAppearing();

        if (BindingContext is BrowseItemsViewModel vm)
        {
            await vm.LoadItemsAsync();
        }
    }
}
