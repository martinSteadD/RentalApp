using RentalApp.Models;

namespace RentalApp.Services;

public class CategoryService
{
    private readonly ApiClient _apiClient;

    public CategoryService(ApiClient apiClient)
    {
        _apiClient = apiClient;
    }

    public async Task<List<Category>> GetCategoriesAsync()
    {
        var response = await _apiClient.GetAsync<CategoryResponse>("categories");
        return response.Categories;
    }
}
