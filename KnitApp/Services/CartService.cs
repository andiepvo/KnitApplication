namespace KnitApp.Services;

using KnitApp.Data;
using KnitApp.Models;
using Microsoft.EntityFrameworkCore;

public class CartService : ICartService
{
    private readonly AppDbContext _context;

    public CartService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<CartItem> AddToCartAsync(int patternId)
    {
        var cartItem = new CartItem { PatternId = patternId, Quantity = 1 };
        _context.CartItems.Add(cartItem);
        await _context.SaveChangesAsync();
        return cartItem;
    }

    public async Task RemoveFromCartAsync(int cartItemId)
    {
        var cartItem = await _context.CartItems.FindAsync(cartItemId);
        if (cartItem != null)
        {
            _context.CartItems.Remove(cartItem);
            await _context.SaveChangesAsync();
        }
    }

    public async Task UpdateQuantityAsync(int cartItemId, int quantity)
    {
        var cartItem = await _context.CartItems.FindAsync(cartItemId);
        if (cartItem != null)
        {
            cartItem.Quantity = quantity;
            await _context.SaveChangesAsync();
        }
    }

    public async Task<List<CartItem>> GetCartAsync() =>
        await _context.CartItems
            .Include(c => c.Pattern)
            .ThenInclude(p => p.Materials)
            .Include(c => c.ExcludedMaterials)
            .ToListAsync();

    public async Task RemoveMaterialFromCartItemAsync(int cartItemId, int materialId)
    {
        var isExcluded = await _context.CartItemExcludedMaterials
            .AnyAsync(e => e.CartItemId == cartItemId && e.MaterialId == materialId);

        if (!isExcluded)
        {
            _context.CartItemExcludedMaterials.Add(new CartItemExcludedMaterial
            {
                CartItemId = cartItemId,
                MaterialId = materialId
            });
            await _context.SaveChangesAsync();
        }
    }
}