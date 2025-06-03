namespace Service.Models.Reqests;

public class ChapterUpdateRequest
{
    public int Id { get; set; }
    public string? ChapterKeyword { get; set; }
    public double? ChapterNumber { get; set; }
    public string? Title { get; set; }
    public string? Url { get; set; }
    public bool? Downloaded { get; set; }
}
