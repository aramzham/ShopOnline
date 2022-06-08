using System.Net;
using System.Net.Http.Json;
using ShopOnline.Models.Dtos;
using ShopOnline.Web.Services.Contracts;

namespace ShopOnline.Web.Services;

public class ProductService : IProductService
{
    private readonly HttpClient _httpClient;

    public ProductService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }
    
    public async Task<IEnumerable<ProductDto>> GetItems()
    {
        try
        {
            var response = await _httpClient.GetAsync("api/Product");
            if (response.IsSuccessStatusCode)
            {
                if (response.StatusCode == HttpStatusCode.NoContent)
                    return Enumerable.Empty<ProductDto>();

                return await response.Content.ReadFromJsonAsync<IEnumerable<ProductDto>>();
            }

            var message = await response.Content.ReadAsStringAsync();
            throw new Exception(message);
        }
        catch
        {
            // log here
            throw;
        }
    }
    
    public async Task<ProductDto> GetItem(int id)
    {
        try
        {
            var response = await _httpClient.GetAsync($"api/Product/{id}");

            if (response.IsSuccessStatusCode)
            {
                if (response.StatusCode == HttpStatusCode.NoContent)
                    return default;

                return await response.Content.ReadFromJsonAsync<ProductDto>();
            }

            var message = await response.Content.ReadAsStringAsync();
            throw new Exception(message);
        }
        catch
        {
            // log here
            throw;
        }
    }

    public async Task<IEnumerable<ProductCategoryDto>> GetProductCategories()
    {
        try
        {
            var response = await _httpClient.GetAsync("api/Product/GetProductCategories");

            if (response.IsSuccessStatusCode)
            {
                if (response.StatusCode == HttpStatusCode.NoContent)
                    return Enumerable.Empty<ProductCategoryDto>();

                return await response.Content.ReadFromJsonAsync<IEnumerable<ProductCategoryDto>>();
            }

            var message = await response.Content.ReadAsStringAsync();
            throw new Exception($"Http Status Code - {response.StatusCode} Message - {message}");
        }
        catch (Exception e)
        {
            // log exception
            throw;
        }
    }

    public async Task<IEnumerable<ProductDto>> GetItemsByCategory(int categoryId)
    {
        try
        {
            var response = await _httpClient.GetAsync($"api/Product/{categoryId}/GetItemsByCategory");

            if (response.IsSuccessStatusCode)
            {
                if (response.StatusCode == HttpStatusCode.NoContent)
                    return Enumerable.Empty<ProductDto>();

                return await response.Content.ReadFromJsonAsync<IEnumerable<ProductDto>>();
            }

            var message = await response.Content.ReadAsStringAsync();
            throw new Exception($"Http Status Code - {response.StatusCode} Message - {message}");
        }
        catch (Exception e)
        {
            // log exception
            throw;
        }
    }
}