using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using ShopOnline.Models.Dtos;
using ShopOnline.Web.Services.Contracts;

namespace ShopOnline.Web.Pages;

public class CheckoutBase : ComponentBase
{
    [Inject] public IJSRuntime Js { get; set; }
    [Inject] public IShoppingCartService ShoppingCartService { get; set; }
    [Inject] public IManageCartItemsLocalStorageService ManageCartItemsLocalStorageService { get; set; }
    
    protected IEnumerable<CartItemDto> ShoppingCartItems { get; set; }
    protected int TotalQty { get; set; }
    protected string PaymentDescription { get; set; }
    protected decimal PaymentAmount { get; set; }

    protected override async Task OnInitializedAsync()
    {
        try
        {
            ShoppingCartItems = await ManageCartItemsLocalStorageService.GetCollection();

            if (ShoppingCartItems is not null)
            {
                var orderGuid = Guid.NewGuid();

                PaymentAmount = ShoppingCartItems.Sum(x => x.TotalPrice);
                TotalQty = ShoppingCartItems.Sum(x => x.Qty);
                PaymentDescription = $"O_{HardCoded.UserId}_{orderGuid}";
            }
        }
        catch (Exception e)
        {
            // log
            throw;
        }
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        try
        {
            if (firstRender)
            {
                await Js.InvokeVoidAsync("initPayPalButton");
            }
        }
        catch (Exception e)
        {
            throw;
        }
    }
}