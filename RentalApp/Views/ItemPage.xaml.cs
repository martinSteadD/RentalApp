using RentalApp.ViewModels;

namespace RentalApp.Views;

public partial class ItemPage : ContentPage
{
    public ItemPage(ItemViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}
