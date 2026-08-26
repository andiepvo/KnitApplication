using KnitApp.Models;

namespace KnitApp.Services;

public interface IPatternImageService
{
    Task<PatternImage> SaveImageAsync(int patternId, Stream fileStream, string fileName);
    Task<List<PatternImage>> GetImagesForPatternAsync(int patternId);
    Task DeleteImageAsync(int id);
}