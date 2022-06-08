using Microsoft.AspNetCore.Components;
using ShopOnline.Models.Dtos;
using ShopOnline.Web.Services.Contracts;

namespace ShopOnline.Web.Pages;

public class ProductDetailsBase : ComponentBase
{
    [Parameter] public int Id { get; set; }
    [Inject] public IProductService ProductService { get; set; }
    [Inject] public IShoppingCartService ShoppingCartService { get; set; }
    [Inject] public NavigationManager NavigationManager { get; set; }
    
    [Inject] public IManageProductsLocalStorageService ManageProductsLocalStorageService { get; set; }
    
    [Inject] public IManageCartItemsLocalStorageService ManageCartItemsLocalStorageService { get; set; }
    
    public string ErrorMessage { get; set; }
    public ProductDto Product { get; set; }

    private List<CartItemDto> ShoppingCartItems { get; set; }

    protected override async Task OnInitializedAsync()
    {
        try
        {
            ShoppingCartItems = await ManageCartItemsLocalStorageService.GetCollection();
            Product = await GetProductById(Id);
        }
        catch (Exception e)
        {
            ErrorMessage = e.Message;
        }
    }

    protected async Task AddToCart_Click(CartItemToAddDto cartItemToAddDto)
    {
        try
        {
            var cartItemDto = await ShoppingCartService.AddItem(cartItemToAddDto);

            if (cartItemDto is not null)
            {
                ShoppingCartItems.Add(cartItemDto);
                await ManageCartItemsLocalStorageService.SaveCollection(ShoppingCartItems);
            }

            NavigationManager.NavigateTo("/ShoppingCart");
        }
        catch (Exception e)
        {
            // log
        }
    }

    private async Task<ProductDto> GetProductById(int id)
    {
        var productDtos = await ManageProductsLocalStorageService.GetCollection();

        return productDtos?.FirstOrDefault(x => x.Id == id);
    }
}