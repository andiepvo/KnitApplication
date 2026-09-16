namespace KnitApp.Services;

using KnitApp.Models;

public interface ICartService
{
    Task<CartItem> AddToCartAsync(int patternId);
    Task RemoveFromCartAsync(int cartItemId);
    Task UpdateQuantityAsync(int cartItemId, int quantity);
    Task<List<CartItem>> GetCartAsync();
}