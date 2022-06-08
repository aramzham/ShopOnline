using Blazored.LocalStorage;
using ShopOnline.Models.Dtos;
using ShopOnline.Web.Services.Contracts;

namespace ShopOnline.Web.Services;

public class ManageProductsLocalStorageService : IManageProductsLocalStorageService
{
    private readonly ILocalStorageService _localStorageService;
    private readonly IProductService _productService;

    private const string Key = "ProductCollection";
    
    public ManageProductsLocalStorageService(ILocalStorageService localStorageService, IProductService productService)
    {
        _localStorageService = localStorageService;
        _productService = productService;
    }
    
    public async Task<IEnumerable<ProductDto>> GetCollection()
    {
        return await _localStorageService.GetItemAsync<IEnumerable<ProductDto>>(Key)
            ?? await AddCollection();
    }

    public ValueTask RemoveCollection()
    {
        return _localStorageService.RemoveItemAsync(Key);
    }

    private async Task<IEnumerable<ProductDto>> AddCollection()
    {
        var productCollection = await _productService.GetItems();

        if (productCollection is { })
        {
            await _localStorageService.SetItemAsync(Key, productCollection);
        }

        return productCollection;
    }
}