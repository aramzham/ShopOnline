using Blazored.LocalStorage;
using ShopOnline.Models.Dtos;
using ShopOnline.Web.Services.Contracts;

namespace ShopOnline.Web.Services;

public class ManageCartItemsLocalStorageService : IManageCartItemsLocalStorageService
{
    private readonly ILocalStorageService _localStorageService;
    private readonly IShoppingCartService _shoppingCartService;

    private const string Key = "ShoppingCartCollection";

    public ManageCartItemsLocalStorageService(ILocalStorageService localStorageService, IShoppingCartService shoppingCartService)
    {
        _localStorageService = localStorageService;
        _shoppingCartService = shoppingCartService;
    }
    
    public async Task<List<CartItemDto>> GetCollection()
    {
        return await _localStorageService.GetItemAsync<List<CartItemDto>>(Key)
               ?? await AddCollection();
    }

    public ValueTask SaveCollection(List<CartItemDto> cartItemDtos)
    {
        return _localStorageService.SetItemAsync(Key, cartItemDtos);
    }

    public ValueTask RemoveCollection()
    {
        return _localStorageService.RemoveItemAsync(Key);
    }

    private async Task<List<CartItemDto>> AddCollection()
    {
        var shoppingCartCollection = await _shoppingCartService.GetItems(HardCoded.UserId);

        if (shoppingCartCollection is not null)
        {
            await _localStorageService.SetItemAsync(Key, shoppingCartCollection);
        }

        return shoppingCartCollection;
    }
}