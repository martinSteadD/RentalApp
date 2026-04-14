using RentalApp.ViewModels;

namespace RentalApp.Views;

public partial class MyItemsPage : ContentPage
{
    public MyItemsPage(MyItemsViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }
}
