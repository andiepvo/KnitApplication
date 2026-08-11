using KnitApp.Data;
using KnitApp.Models;
using Microsoft.EntityFrameworkCore;


namespace KnitApp.Services;

//this class is implementing the interface PatternService
public class PatternService: IPatternService 
{
    //fungerer som en type lagringsplass for hvert objekt i denne klassen
    private readonly AppDbContext _context;
    
    public PatternService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<Pattern>> GetAllAsync() 
    { 
        return await _context.Patterns
            .Include(p => p.Materials)
            .ToListAsync();
    }

    public async Task<Pattern?> GetByIdAsync(int id)
    {
        return await _context.Patterns.FindAsync(id); 

    }

    public async Task<Pattern> AddAsync(Pattern pattern)
    {
        _context.Patterns.Add(pattern);
        await _context.SaveChangesAsync();
        return pattern;

    }
    public async Task<Pattern> UpdateAsync(Pattern pattern)
    {
        _context.Patterns.Update(pattern);
        await _context.SaveChangesAsync();
        return pattern;

    }
    public async Task DeleteAsync(int id) 
    {
        var x = await _context.Patterns.FindAsync(id);
        if (x != null)
        {
            _context.Patterns.Remove(x);
            await _context.SaveChangesAsync();
        }
        

    }
} 