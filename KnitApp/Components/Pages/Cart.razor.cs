using Microsoft.AspNetCore.Components;
using KnitApp.Services;
using KnitApp.Models;

namespace KnitApp.Components.Pages;

public partial class Cart
{
    [Inject]
    private ICartService CartService { get; set; } = default!;

    [Inject]
    private IShoppingListService ShoppingListService { get; set; } = default!;
    
    [Inject]
    private IPatternImageService PatternImageService { get; set; } = default!;
    
    private List<CartItem> cartItems = new();
    private Dictionary<int, string?> patternThumbnails = new();
    private string activeTab = "recipe";
    
    protected override async Task OnInitializedAsync() =>
        await LoadCart();

    private async Task LoadCart()
    {
        cartItems = await CartService.GetCartAsync();
        await LoadThumbnails();
    }
    
    private async Task LoadThumbnails()
    {
        patternThumbnails.Clear();
        foreach (var patternId in cartItems.Select(c => c.Pattern.Id).Distinct())
        {
            var images = await PatternImageService.GetImagesForPatternAsync(patternId);
            patternThumbnails[patternId] = images.FirstOrDefault()?.FilePath;
        }
    }
    
    private void ShowRecipeTab() => activeTab = "recipe";

    private void ShowIngredientsTab() => activeTab = "ingredients";

    private async Task HandleRemovePattern(int cartItemId)
    {
        await CartService.RemoveFromCartAsync(cartItemId);
        await LoadCart();
    }

    private async Task HandleRemoveMaterial(int cartItemId, int materialId)
    {
        await CartService.RemoveMaterialFromCartItemAsync(cartItemId, materialId);
        await LoadCart();
    }

    private async Task IncreaseQuantity(CartItem item)
    {
        await CartService.UpdateQuantityAsync(item.Id, item.Quantity + 1);
        await LoadCart();
    }

    private async Task DecreaseQuantity(CartItem item)
    {
        if (item.Quantity > 1)
        {
            await CartService.UpdateQuantityAsync(item.Id, item.Quantity - 1);
            await LoadCart();
        }
    }

    private List<ShoppingListItemDto> GetAggregatedList()
    {
        var expandedPatterns = cartItems.SelectMany(item => Enumerable.Repeat(
            new Pattern
            {
                Materials = item.Pattern.Materials
                    .Where(m => !item.ExcludedMaterials.Any(e => e.MaterialId == m.Id))
                    .ToList()
            },
            item.Quantity)).ToList();

        return ShoppingListService.GenerateShoppingList(expandedPatterns);
    }
    
    private async Task HandleRemoveAggregatedMaterial(string yarnName)
    {
        foreach (var item in cartItems)
        {
            var matches = item.Pattern.Materials
                .Where(m => string.Equals(m.MaterialName, yarnName, StringComparison.OrdinalIgnoreCase))
                .Where(m => !item.ExcludedMaterials.Any(e => e.MaterialId == m.Id));

            foreach (var material in matches)
            {
                await CartService.RemoveMaterialFromCartItemAsync(item.Id, material.Id);
            }
        }
        await LoadCart();
    }
    
    private static string Pluralize(string unit, int quantity)
    {
        if (quantity == 1 && unit.EndsWith("s", StringComparison.OrdinalIgnoreCase) && unit.Length > 1)
        {
            return unit[..^1];
        }
        return unit;
    }
}