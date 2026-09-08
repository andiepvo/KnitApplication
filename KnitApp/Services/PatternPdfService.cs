namespace KnitApp.Services;

public class PatternPdfService : IPatternPdfService
{
    private readonly IWebHostEnvironment _environment;

    public PatternPdfService(IWebHostEnvironment environment)
    {
        _environment = environment;
    }

    public async Task<string> SavePdfAsync(Stream fileStream, string fileName)
    {
        var uploadsFolder = Path.Combine(_environment.WebRootPath, "uploads", "pattern-pdfs");
        Directory.CreateDirectory(uploadsFolder);

        var uniqueFileName = $"{Guid.NewGuid()}{Path.GetExtension(fileName)}";
        var fullPath = Path.Combine(uploadsFolder, uniqueFileName);

        await using (var outputStream = new FileStream(fullPath, FileMode.Create))
        {
            await fileStream.CopyToAsync(outputStream);
        }

        return $"/uploads/pattern-pdfs/{uniqueFileName}";
    }
}