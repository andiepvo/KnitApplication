using Microsoft.AspNetCore.Components;
using KnitApp.Services;

namespace KnitApp.Components.Layout;

public partial class NavMenu
{
    [Inject]
    private ICartService CartService { get; set; } = default!;
    
    private int cartItemCount;
    
    protected override async Task OnInitializedAsync()
    {
        var cart = await CartService.GetCartAsync();
        cartItemCount = cart.Sum(c => c.Quantity);
    }
}