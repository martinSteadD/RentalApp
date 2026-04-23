using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using RentalApp.Database.Models;
using RentalApp.Database.Repositories;
using System.Collections.ObjectModel;

namespace RentalApp.ViewModels;

public partial class ItemViewModel : BaseViewModel
{
    private readonly IItemRepository itemRepository;

    public ObservableCollection<Item> Items { get; } = new();

    [ObservableProperty]
    private string name;

    [ObservableProperty]
    private string description;

    public ItemViewModel(IItemRepository itemRepository)
    {
        this.itemRepository = itemRepository;
    }

    [RelayCommand]
    private async Task LoadItemsAsync()
    {
        try
        {
            IsBusy = true;
            HasError = false;

            var items = await itemRepository.GetAllAsync();
            Items.Clear();
            foreach (var item in items)
                Items.Add(item);
        }
        catch (Exception ex)
        {
            ErrorMessage = $"Load failed: {ex.Message}";
            HasError = true;
        }
        finally
        {
            IsBusy = false;
        }
    }

    [RelayCommand]
    private async Task AddItemAsync()
    {
        try
        {
            IsBusy = true;
            HasError = false;

            var newItem = new Item { Name = Name, Description = Description };
            await itemRepository.AddAsync(newItem);
            await LoadItemsAsync();

            Name = string.Empty;
            Description = string.Empty;
        }
        catch (Exception ex)
        {
            ErrorMessage = $"Add failed: {ex.Message}";
            HasError = true;
        }
        finally
        {
            IsBusy = false;
        }
    }

    [RelayCommand]
    private async Task EditItemAsync()
    {
        try
        {
            IsBusy = true;
            HasError = false;

            var item = Items.FirstOrDefault(i => i.Name == Name);
            if (item != null)
            {
                item.Description = Description;
                await itemRepository.UpdateAsync(item);
                await LoadItemsAsync();

                Name = string.Empty;
                Description = string.Empty;
            }
            else
            {
                ErrorMessage = "Item not found for editing.";
                HasError = true;
            }
        }
        catch (Exception ex)
        {
            ErrorMessage = $"Edit failed: {ex.Message}";
            HasError = true;
        }
        finally
        {
            IsBusy = false;
        }
    }

    [RelayCommand]
    private async Task DeleteItemAsync()
    {
        try
        {
            IsBusy = true;
            HasError = false;

            var item = Items.FirstOrDefault(i => i.Name == Name);
            if (item != null)
            {
                await itemRepository.DeleteAsync(item.Id);
                await LoadItemsAsync();

                Name = string.Empty;
                Description = string.Empty;
            }
            else
            {
                ErrorMessage = "Item not found for deletion.";
                HasError = true;
            }
        }
        catch (Exception ex)
        {
            ErrorMessage = $"Delete failed: {ex.Message}";
            HasError = true;
        }
        finally
        {
            IsBusy = false;
        }
    }
}
