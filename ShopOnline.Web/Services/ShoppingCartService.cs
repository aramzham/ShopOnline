using System.Net;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using ShopOnline.Models.Dtos;
using ShopOnline.Web.Services.Contracts;

namespace ShopOnline.Web.Services;

public class ShoppingCartService : IShoppingCartService
{
    private readonly HttpClient _httpClient;
    
    public event Action<int> OnShoppingCartChanged;

    public ShoppingCartService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<List<CartItemDto>> GetItems(int userId)
    {
        try
        {
            var response = await _httpClient.GetAsync($"api/ShoppingCart/{userId}/GetItems");
            
            if (response.IsSuccessStatusCode)
            {
                if (response.StatusCode == HttpStatusCode.NoContent)
                    return new List<CartItemDto>();

                return await response.Content.ReadFromJsonAsync<List<CartItemDto>>();
            }
            else
            {
                var message = await response.Content.ReadAsStringAsync();
                throw new Exception($"Http status: {response.StatusCode} Message -{message}");
            }
        }
        catch
        {
            // log
            throw;
        }
    }

    public async Task<CartItemDto> AddItem(CartItemToAddDto cartItemToAddDto)
    {
        try
        {
            var response = await _httpClient.PostAsJsonAsync("api/ShoppingCart", cartItemToAddDto);

            if (response.IsSuccessStatusCode)
            {
                if (response.StatusCode == HttpStatusCode.NoContent)
                    return default;

                return await response.Content.ReadFromJsonAsync<CartItemDto>();
            }
            else
            {
                var message = await response.Content.ReadAsStringAsync();
                throw new Exception($"Http status: {response.StatusCode} Message -{message}");
            }
        }
        catch
        {
            // log
            throw;
        }
    }

    public async Task<CartItemDto> DeleteItem(int id)
    {
        try
        {
            var response = await _httpClient.DeleteAsync($"api/ShoppingCart/{id}");

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<CartItemDto>();
            }

            return default;
        }
        catch (Exception e)
        {
            // log
            throw;
        }
    }

    public async Task<CartItemDto> UpdateQty(CartItemQtyUpdateDto cartItemQtyUpdateDto)
    {
        try
        {
            var jsonRequest = JsonSerializer.Serialize(cartItemQtyUpdateDto);
            var content = new StringContent(jsonRequest, Encoding.UTF8, "application/json-patch+json");

            var response = await _httpClient.PatchAsync($"api/ShoppingCart/{cartItemQtyUpdateDto.CartItemId}", content);

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<CartItemDto>();
            }

            return null;
        }
        catch (Exception e)
        {
            throw;
        }
    }
    
    public void RaiseEventOnShoppingCartChanged(int totalQty)
    {
        OnShoppingCartChanged?.Invoke(totalQty);
    }
}