using Microsoft.AspNetCore.Components;
using KnitApp.Services;
using KnitApp.Models;

namespace KnitApp.Components.Pages;

public partial class DetailPattern
{
    [Inject] 
    private IPatternService PatternService { get; set; } = default!;
    
    [Inject] 
    private ICartService CartService { get; set; } = default!;
    
    [Inject] 
    private IPatternImageService PatternImageService { get; set; } = default!;
    
    [Inject]
    private NavigationManager NavigationManager { get; set; } = default!;

    private void GoBack() => NavigationManager.NavigateTo("/patterns");
    
    [Parameter]  
    public int Id { get; set; }

    private Pattern? pattern;
    private string? toastMessage;
    
    private List<string> heroImages = new();

    protected override async Task OnInitializedAsync()
    {
        pattern = await PatternService.GetByIdAsync(Id);

        var images = await PatternImageService.GetImagesForPatternAsync(Id);
        heroImages = images
            .Select(i => i.FilePath)
            .Where(p => !string.IsNullOrEmpty(p))
            .ToList()!;
    }

    private async Task AddToCart()
    {
        await CartService.AddToCartAsync(Id);
        toastMessage = $"{pattern?.Name} added to the cart";
    }
    
    private void CloseToast()
    {
        toastMessage = null;
    }
    
}