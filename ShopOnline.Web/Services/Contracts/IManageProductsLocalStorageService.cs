using ShopOnline.Models.Dtos;

namespace ShopOnline.Web.Services.Contracts;

public interface IManageProductsLocalStorageService
{
    Task<IEnumerable<ProductDto>> GetCollection();
    ValueTask RemoveCollection();
}