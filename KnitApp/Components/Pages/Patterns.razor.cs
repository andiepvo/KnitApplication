using Microsoft.AspNetCore.Components;
using KnitApp.Services;
using KnitApp.Models;

namespace KnitApp.Components.Pages;

public partial class Patterns
{
    // Injected dependencies
    [Inject]
    private IPatternService PatternService { get; set; } = default!;

    [Inject]
    private ICartService CartService { get; set; } = default!;

    [Inject]
    private IPatternImageService PatternImageService { get; set; } = default!;

    [Inject]
    private NavigationManager NavigationManager { get; set; } = default!;

    // Fields
    private List<Pattern> patterns = new();
    private Dictionary<int, string?> patternThumbnails = new();
    private string? toastMessage;

    // Lifecycle methods
    protected override async Task OnInitializedAsync()
    {
        patterns = await PatternService.GetAllAsync();
        await LoadThumbnails();
    }

    private async Task LoadThumbnails()
    {
        patternThumbnails.Clear();
        foreach (var pattern in patterns)
        {
            var images = await PatternImageService.GetImagesForPatternAsync(pattern.Id);
            patternThumbnails[pattern.Id] = images.FirstOrDefault()?.FilePath;
        }
    }

    // Event handlers
    private async Task HandleDelete(int id)
    {
        await PatternService.DeleteAsync(id);
        patterns = await PatternService.GetAllAsync();
        await LoadThumbnails();
    }

    private void GoToEdit(int id)
    {
        NavigationManager.NavigateTo($"/editpatterns/{id}");
    }

    private void GoToDetails(int id)
    {
        NavigationManager.NavigateTo($"/detailpatterns/{id}");
    }

    private async Task HandleAddToCart(int id)
    {
        await CartService.AddToCartAsync(id);

        var pattern = patterns.First(p => p.Id == id);
        var cart = await CartService.GetCartAsync();
        var totalItems = cart.Sum(c => c.Quantity);

        toastMessage = $"{pattern.Name} added to cart! ({totalItems} items in cart)";
    }

    private void CloseToast()
    {
        toastMessage = null;
    }

    private void GoToCart()
    {
        NavigationManager.NavigateTo("/cart");
    }
}