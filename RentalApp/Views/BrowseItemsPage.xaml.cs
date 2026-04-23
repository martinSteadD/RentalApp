using RentalApp.ViewModels;
using RentalApp.Models;

namespace RentalApp.Views;

public partial class BrowseItemsPage : ContentPage
{
    private Border? _previousSelectedBorder;

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

    private void OnSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        // Reset previous highlight
        if (_previousSelectedBorder != null)
            _previousSelectedBorder.BackgroundColor = Colors.White;

        // Get selected item
        var selectedItem = e.CurrentSelection.FirstOrDefault();
        if (selectedItem == null)
            return;

        // Find the Border for highlight
        var container = FindBorderForItem(selectedItem);
        if (container != null)
        {
            container.BackgroundColor = Color.FromArgb("#E8D9FF");
            _previousSelectedBorder = container;
        }

        // Trigger navigation
        if (BindingContext is BrowseItemsViewModel vm)
        {
            vm.SelectItemCommand.Execute(selectedItem);
        }
    }

    private Border? FindBorderForItem(object item)
    {
        foreach (var visual in GetAllVisualChildren(ItemsCollection))
        {
            if (visual.BindingContext == item)
            {
                return visual.FindByName<Border>("ItemBorder");
            }
        }

        return null;
    }

    private IEnumerable<VisualElement> GetAllVisualChildren(VisualElement parent)
    {
        if (parent is null)
            yield break;

        var queue = new Queue<VisualElement>();
        queue.Enqueue(parent);

        while (queue.Count > 0)
        {
            var element = queue.Dequeue();

            if (element is Layout layout)
            {
                foreach (var child in layout.Children)
                {
                    if (child is VisualElement ve)
                    {
                        queue.Enqueue(ve);
                        yield return ve;
                    }
                }
            }
            else if (element is ContentView cv && cv.Content is VisualElement content)
            {
                queue.Enqueue(content);
                yield return content;
            }
        }
    }
}
