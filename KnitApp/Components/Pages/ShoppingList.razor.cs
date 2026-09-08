using Microsoft.AspNetCore.Components;
using KnitApp.Services;
using KnitApp.Models;

namespace KnitApp.Components.Pages;

public partial class ShoppingList
{
    // Injected dependencies
    [Inject]
    private IPatternService PatternService { get; set; } = default!;

    [Inject]
    private IShoppingListService ShoppingListService { get; set; } = default!;

    // Fields
    private List<Pattern> patterns = new();

    //Which pattern is chosen
    private HashSet<int> selectedPatternIds = new HashSet<int>();

    private List<ShoppingListItemDto> shoppingList = new();

    // Lifecycle methods
    protected override async Task OnInitializedAsync() =>
        patterns = await PatternService.GetAllAsync();

    // Event handlers
    private void ToggleSelection(int id, bool isChecked)
    {
        if (isChecked)
            selectedPatternIds.Add(id);
        else
            selectedPatternIds.Remove(id);
    }

    private void GenerateList()
    {
        var selectedPatterns = patterns
            .Where(p => selectedPatternIds.Contains(p.Id))
            .ToList();

        shoppingList = ShoppingListService.GenerateShoppingList(selectedPatterns);
    }
}