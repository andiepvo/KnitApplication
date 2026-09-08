namespace KnitApp.Services;

public interface IPatternPdfService
{
    Task<string> SavePdfAsync(Stream fileStream, string fileName);
}