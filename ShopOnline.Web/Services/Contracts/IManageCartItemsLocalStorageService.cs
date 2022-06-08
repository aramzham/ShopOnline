using ShopOnline.Models.Dtos;

namespace ShopOnline.Web.Services.Contracts;

public interface IManageCartItemsLocalStorageService
{
    Task<List<CartItemDto>> GetCollection();
    ValueTask SaveCollection(List<CartItemDto> cartItemDtos);
    ValueTask RemoveCollection();
}