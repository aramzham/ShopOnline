using Microsoft.EntityFrameworkCore;
using ShopOnline.Api.Data;
using ShopOnline.Api.Entities;
using ShopOnline.Api.Repositories.Contracts;

namespace ShopOnline.Api.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly ShopOnlineDbContext _shopOnlineDbContext;

        public ProductRepository(ShopOnlineDbContext shopOnlineDbContext)
        {
            _shopOnlineDbContext = shopOnlineDbContext;
        }

        public async Task<IEnumerable<ProductCategory>> GetCategories()
        {
            var categories = await _shopOnlineDbContext.ProductCategories.ToListAsync();

            return categories;
        }

        public Task<ProductCategory> GetCategory(int id)
        {
            return _shopOnlineDbContext.ProductCategories.FirstOrDefaultAsync(c => c.Id == id);
        }

        public Task<Product> GetItem(int id)
        {
            return _shopOnlineDbContext.Products
                .Include(p => p.ProductCategory)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<IEnumerable<Product>> GetItems()
        {
            return await _shopOnlineDbContext.Products.Include(p => p.ProductCategory).ToListAsync();
        }

        public async Task<IEnumerable<Product>> GetItemsByCategory(int id)
        {
            return await _shopOnlineDbContext.Products
                .Include(p => p.ProductCategory)
                .Where(p => p.CategoryId == id).ToListAsync();
        }
    }
}