﻿using Microsoft.EntityFrameworkCore;
using ShopOnline.Api.Data;
using ShopOnline.Api.Entities;
using ShopOnline.Api.Repositories.Contracts;
using ShopOnline.Models.Dtos;

namespace ShopOnline.Api.Repositories
{
    public class ShoppingCartRepository : IShoppingCartRepository
    {
        private readonly ShopOnlineDbContext _shopOnlineDbContext;

        public ShoppingCartRepository(ShopOnlineDbContext shopOnlineDbContext)
        {
            _shopOnlineDbContext = shopOnlineDbContext;
        }

        private Task<bool> CartItemExists(int cartId, int productId)
        {
            return _shopOnlineDbContext.CartItems.AnyAsync(c => c.CartId == cartId && c.ProductId == productId);
        }

        public async Task<CartItem> AddItem(CartItemToAddDto cartItemToAddDto)
        {
            if (await CartItemExists(cartItemToAddDto.CartId, cartItemToAddDto.ProductId) == false)
            {
                var item = await _shopOnlineDbContext.Products
                    .Where(product => product.Id == cartItemToAddDto.ProductId)
                    .Select(product => new CartItem
                    {
                        CartId = cartItemToAddDto.CartId, ProductId = product.Id, Qty = cartItemToAddDto.Qty
                    }).SingleOrDefaultAsync();

                if (item != null)
                {
                    var result = await _shopOnlineDbContext.CartItems.AddAsync(item);
                    await _shopOnlineDbContext.SaveChangesAsync();
                    return result.Entity;
                }
            }

            return null;
        }

        public async Task<CartItem> DeleteItem(int id)
        {
            var item = await _shopOnlineDbContext.CartItems.FindAsync(id);

            if (item != null)
            {
                _shopOnlineDbContext.CartItems.Remove(item);
                await _shopOnlineDbContext.SaveChangesAsync();
            }

            return item;
        }

        public async Task<CartItem> GetItem(int id)
        {
            return await _shopOnlineDbContext.Carts
                .Join(_shopOnlineDbContext.CartItems, cart => cart.Id, cartItem => cartItem.CartId,
                    (cart, cartItem) => new { cart, cartItem })
                .Where(t => t.cartItem.Id == id)
                .Select(t => new CartItem
                {
                    Id = t.cartItem.Id,
                    ProductId = t.cartItem.ProductId,
                    Qty = t.cartItem.Qty,
                    CartId = t.cartItem.CartId
                }).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<CartItem>> GetItems(int userId)
        {
            return await _shopOnlineDbContext.Carts
                .Join(_shopOnlineDbContext.CartItems, cart => cart.Id, cartItem => cartItem.CartId,
                    (cart, cartItem) => new { cart, cartItem })
                .Where(t => t.cart.UserId == userId)
                .Select(t => new CartItem
                {
                    Id = t.cartItem.Id,
                    ProductId = t.cartItem.ProductId,
                    Qty = t.cartItem.Qty,
                    CartId = t.cartItem.CartId
                }).ToListAsync();
        }

        public async Task<CartItem> UpdateQty(int id, CartItemQtyUpdateDto cartItemQtyUpdateDto)
        {
            var item = await _shopOnlineDbContext.CartItems.FindAsync(id);

            if (item != null)
            {
                item.Qty = cartItemQtyUpdateDto.Qty;
                await _shopOnlineDbContext.SaveChangesAsync();
                return item;
            }

            return null;
        }
    }
}