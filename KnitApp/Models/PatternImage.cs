namespace KnitApp.Models;

public class PatternImage
{
    public int Id { get; set; }
    public string FilePath { get; set; } = string.Empty;
    public int PatternId { get; set; }
    public Pattern Pattern { get; set; } = null!;
    public DateTime UploadedOn { get; set; }   
}