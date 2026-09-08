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
    private NavigationManager NavigationManager { get; set; } = default!;

    // Fields
    private List<Pattern> patterns = new();

    // Lifecycle methods
    protected override async Task OnInitializedAsync() =>
        patterns = await PatternService.GetAllAsync();

    // Event handlers
    private async Task HandleDelete(int id)
    {
        await PatternService.DeleteAsync(id);
        patterns = await PatternService.GetAllAsync();
    }

    private void GoToEdit(int id)
    {
        NavigationManager.NavigateTo($"/editpatterns/{id}");
    }
}