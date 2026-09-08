using System.Net.Http.Json;
using KnitApp.Models;
using KnitApp.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;

namespace KnitApp.Components.Pages;

public partial class EditPatterns
{
    // Injected dependencies
    [Inject]
    private IHttpClientFactory HttpClientFactory { get; set; } = default!;

    [Inject]
    private NavigationManager Navigation { get; set; } = default!;

    [Inject]
    private IPatternService PatternService { get; set; } = default!;

    [Inject]
    private IYarnCatalogService YarnCatalogService { get; set; } = default!;

    [Inject]
    private IPatternImageService PatternImageService { get; set; } = default!;

    // Fields
    private bool isUploadingPdf = false;

    [SupplyParameterFromForm]
    private Pattern editingPattern { get; set; } = new();

    private List<YarnCatalog> yarnCatalog = new();

    private List<PatternImage> patternImages = new();

    // Parameters
    [Parameter]
    public int Id { get; set; }

    // Lifecycle methods
    protected override async Task OnInitializedAsync()
    {
        yarnCatalog = await YarnCatalogService.GetAllAsync();
        patternImages = await PatternImageService.GetImagesForPatternAsync(Id);

        if (string.IsNullOrEmpty(editingPattern.Name))
        {
            var existing = await PatternService.GetByIdAsync(Id);
            if (existing != null)
            {
                //materials
                editingPattern.Name = existing.Name;
                editingPattern.CraftType = existing.CraftType;
                editingPattern.PatternType = existing.PatternType;
                editingPattern.Description = existing.Description;
                editingPattern.Instructions = existing.Instructions;
                editingPattern.InstructionsPdf = existing.InstructionsPdf;
                editingPattern.Materials = existing.Materials
                    .Select(m => new Material
                    {
                        MaterialName = m.MaterialName,
                        Quantity = m.Quantity,
                        Unit = m.Unit,
                        ColorOfYarn = m.ColorOfYarn
                    })
                    .ToList();

                if (editingPattern.Materials.Count == 0)
                {
                    editingPattern.Materials.Add(new Material());
                }

                //equipment
                editingPattern.Equipment = existing.Equipment
                    .Select(e => new Equipment
                    {
                        EquipmentType = e.EquipmentType,
                        Size = e.Size,
                        Length = e.Length
                    })
                    .ToList();

                if (editingPattern.Equipment.Count == 0)
                {
                    editingPattern.Equipment.Add(new Equipment());
                }
            }
        }
    }

    // Event handlers / other methods
    protected async Task HandleSubmit()
    {
        var pattern = await PatternService.GetByIdAsync(Id);
        if (pattern != null)
        {
            pattern.Name = editingPattern.Name;
            pattern.CraftType = editingPattern.CraftType;
            pattern.PatternType = editingPattern.PatternType;
            pattern.Description = editingPattern.Description;
            pattern.Instructions = editingPattern.Instructions;
            pattern.InstructionsPdf = editingPattern.InstructionsPdf;

            // Ignore empty rows in material section
            pattern.Materials = editingPattern.Materials
                .Where(m => !string.IsNullOrWhiteSpace(m.MaterialName))
                .ToList();

            // Ignore empty rows in equipment section
            pattern.Equipment = editingPattern.Equipment
                .Where(e => !string.IsNullOrWhiteSpace(e.EquipmentType))
                .ToList();

            await PatternService.UpdateAsync(pattern);
            Navigation.NavigateTo("/patterns");
        }
    }

    private void RemoveMaterial(Material material)
    {
        editingPattern.Materials.Remove(material);
        if (editingPattern.Materials.Count == 0)
        {
            editingPattern.Materials.Add(new Material());
        }
    }

    private void AddMaterial()
    {
        editingPattern.Materials.Add(new Material());
    }

    private void ChangeQuantity(Material material, int quantity)
    {
        material.Quantity = Math.Max(0, material.Quantity + quantity);
    }

    private void RemoveEquipment(Equipment equipment)
    {
        editingPattern.Equipment.Remove(equipment);
    }

    private void AddEquipment()
    {
        editingPattern.Equipment.Add(new Equipment());
    }

    private async Task HandleImageUpload(InputFileChangeEventArgs e)
    {
        foreach (var file in e.GetMultipleFiles())
        {
            var stream = file.OpenReadStream(maxAllowedSize: 10 * 1024 * 1024);
            var savedImage = await PatternImageService.SaveImageAsync(Id, stream, file.Name);
            patternImages.Add(savedImage);
        }
    }

    private async Task RemoveImage(PatternImage image)
    {
        await PatternImageService.DeleteImageAsync(image.Id);
        patternImages.Remove(image);
    }

    private async Task HandlePdfUpload(InputFileChangeEventArgs e)
    {
        isUploadingPdf = true;

        var file = e.File;
        var stream = file.OpenReadStream(maxAllowedSize: 20 * 1024 * 1024);

        using var content = new MultipartFormDataContent();
        content.Add(new StreamContent(stream), "file", file.Name);

        var client = HttpClientFactory.CreateClient();
        client.BaseAddress = new Uri(Navigation.BaseUri);

        var response = await client.PostAsync("api/patterns/pdf", content);
        response.EnsureSuccessStatusCode();

        var result = await response.Content.ReadFromJsonAsync<PdfUploadResultDto>();
        editingPattern.InstructionsPdf = result!.FilePath;

        isUploadingPdf = false;
    }
}