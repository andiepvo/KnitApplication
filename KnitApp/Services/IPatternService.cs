using KnitApp.Models;

namespace KnitApp.Services;

//should follow CRUD
public interface IPatternService
{ 
    //get all
    Task<List<Pattern>> GetAllAsync();
    //get one based on id
    Task<Pattern?> GetByIdAsync(int id);
    //make one
    Task<Pattern> AddAsync(Pattern pattern);
    //update
    Task<Pattern> UpdateAsync(Pattern pattern);
    //delete
    Task DeleteAsync(int id);


}