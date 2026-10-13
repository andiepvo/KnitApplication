using KnitApp.Models;
using KnitApp.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;

namespace KnitApp.Components.Pages;

public partial class AddPattern
{
    private string? errorMessage;

    // Injected dependencies
    [Inject] private IHttpClientFactory HttpClientFactory { get; set; } = default!;

    [Inject] private NavigationManager Navigation { get; set; } = default!;

    [Inject] private IPatternService PatternService { get; set; } = default!;

    [Inject] private IYarnCatalogService YarnCatalogService { get; set; } = default!;

    // Fields
    [SupplyParameterFromForm]
    private Pattern newPattern { get; set; } = new()
    {
        Materials = new() { new Material() },
        Equipment = new() { new Equipment() }
    };

    private List<YarnCatalog> yarnCatalog = new();

    private bool isUploadingPdf;
    
    private static readonly string[] UnitOptions = { "Skeins", "Grams", "Meters", "Yards", "Balls" };

    // Lifecycle methods
    protected override async Task OnInitializedAsync()
    {
        yarnCatalog = await YarnCatalogService.GetAllAsync();
    }

    // Event handlers / other methods
    protected async Task HandleSubmit()
    {
        // Ignorer tomme rader brukeren ikke fylte ut
        newPattern.Materials = newPattern.Materials
            .Where(m => !string.IsNullOrWhiteSpace(m.MaterialName))
            .ToList();

        //show actually date and time when the pattern is created
        newPattern.CreatedOn = DateTime.UtcNow;

        try
        {
            await PatternService.AddAsync(newPattern);
            Navigation.NavigateTo("/patterns");
        }
        catch (Exception)
        {
            errorMessage = "Something went wrong while saving the pattern. Please try again.";
        }
    }

    private void RemoveEquipment(Equipment equipment)
    {
        newPattern.Equipment.Remove(equipment);
    }

    private void AddEquipment()
    {
        newPattern.Equipment.Add(new Equipment());
    }

    private void RemoveMaterial(Material material)
    {
        newPattern.Materials.Remove(material);
        if (newPattern.Materials.Count == 0)
        {
            newPattern.Materials.Add(new Material());
        }
    }

    private void AddMaterial()
    {
        newPattern.Materials.Add(new Material());
    }

    private void IncreaseQuantity(Material material)
    {
        material.Quantity += 1;
    }

    private void DecreaseQuantity(Material material)
    {
        if (material.Quantity > 0)
            material.Quantity -= 1;
    }

    private async Task HandlePdfUpload(InputFileChangeEventArgs e)
    {
        isUploadingPdf = true;
        errorMessage = null;

        try
        {
            var file = e.File;
            var stream = file.OpenReadStream(maxAllowedSize: 20 * 1024 * 1024);

            using var content = new MultipartFormDataContent();
            content.Add(new StreamContent(stream), "file", file.Name);

            var client = HttpClientFactory.CreateClient();
            client.BaseAddress = new Uri(Navigation.BaseUri);

            var response = await client.PostAsync("api/patterns/pdf", content);
            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadFromJsonAsync<PdfUploadResultDto>();
            newPattern.InstructionsPdf = result!.FilePath;
        }
        catch (Exception ex)
        {
            errorMessage = "Something went wrong while uploading the PDF. Please try again.";
        }
        finally
        {
            isUploadingPdf = false;
        }
    }
}