using Microsoft.AspNetCore.Components;
using KnitApp.Models;

namespace KnitApp.Components.Shared;

public partial class YarnNameAutocomplete
{
    // Fields
    private bool showSuggestions = false; //the dropdownmenu close when a value is selected

    // Parameters
    [Parameter]
    public string Value { get; set; } = string.Empty;

    [Parameter]
    public EventCallback<string> ValueChanged { get; set; }

    //the actual list with different yarntypes
    [Parameter]
    public List<YarnCatalog> Suggestions { get; set; } = new();

    //filtering data
    private List<YarnCatalog> FilteredSuggestions =>
        string.IsNullOrWhiteSpace(Value)
            ? new List<YarnCatalog>()
            : Suggestions.Where(y => y.Name.Contains(Value, StringComparison.OrdinalIgnoreCase)).ToList();

    // Event handlers
    //handle users input
    private async Task OnInput(ChangeEventArgs e)
    {
        Value = e.Value?.ToString() ?? string.Empty;
        await ValueChanged.InvokeAsync(Value);
        showSuggestions = true;
    }

    //handle the object that is clicked on
    private async Task SelectSuggestion(YarnCatalog suggestion)
    {
        Value = suggestion.Name;
        await ValueChanged.InvokeAsync(Value);
        showSuggestions = false;
    }
}