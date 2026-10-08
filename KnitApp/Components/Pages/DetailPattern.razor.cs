using Microsoft.AspNetCore.Components;
using KnitApp.Services;
using KnitApp.Models;

namespace KnitApp.Components.Pages;

public partial class DetailPattern
{
    [Inject] 
    private IPatternService PatternService { get; set; } = default!;
    
    [Parameter]  
    public int Id { get; set; }

    private Pattern? pattern;

    protected override async Task OnInitializedAsync()
    {
        pattern = await PatternService.GetByIdAsync(Id);
    }

    [Inject] 
    private ICartService CartService { get; set; } = default!;
    
    private string? toastMessage;

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