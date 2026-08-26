using KnitApp.Data;
using KnitApp.Models;
using Microsoft.EntityFrameworkCore;

namespace KnitApp.Services;

public class PatternImageService : IPatternImageService
{
    private readonly AppDbContext _context;
    private readonly IWebHostEnvironment _environment;

    public PatternImageService(AppDbContext context, IWebHostEnvironment environment)
    {
        _context = context;
        _environment = environment;
    }

    public async Task<PatternImage> SaveImageAsync(int patternId, Stream fileStream, string fileName)
    {
        var uploadsFolder = Path.Combine(_environment.WebRootPath, "uploads", "patterns");
        Directory.CreateDirectory(uploadsFolder);

        var uniqueFileName = $"{Guid.NewGuid()}{Path.GetExtension(fileName)}";
        var fullPath = Path.Combine(uploadsFolder, uniqueFileName);

        await using (var outputStream = new FileStream(fullPath, FileMode.Create))
        {
            await fileStream.CopyToAsync(outputStream);
        }

        var patternImage = new PatternImage
        {
            PatternId = patternId,
            FilePath = $"/uploads/patterns/{uniqueFileName}",
            UploadedOn = DateTime.Now //is this right?
        };

        _context.PatternImages.Add(patternImage);
        await _context.SaveChangesAsync();

        return patternImage;
    }

    public async Task<List<PatternImage>> GetImagesForPatternAsync(int patternId)
    {
        return await _context.PatternImages
            .Where(img => img.PatternId == patternId)
            .ToListAsync();
    }

    public async Task DeleteImageAsync(int id)
    {
        var image = await _context.PatternImages.FindAsync(id);
        if (image != null)
        {
            var fullPath = Path.Combine(_environment.WebRootPath, image.FilePath.TrimStart('/'));
            if (File.Exists(fullPath))
            {
                File.Delete(fullPath);
            }

            _context.PatternImages.Remove(image);
            await _context.SaveChangesAsync();
        }
    }
}