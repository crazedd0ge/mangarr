namespace Service.Models.Requests;

public class MangaRequest
{
    public string? Title { get; set; }
    public string? Url { get; set; }
    public string? Description { get; set; }
    public string? CoverImage { get; set; }
    public bool? Complete { get; set; }
}
