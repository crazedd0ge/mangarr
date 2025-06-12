namespace Service.Models.Requests;

public class MangaUpdateRequest
{
    public int Id { get; set; }
    public string? Title { get; set; }
    public string? Url { get; set; }
    public string? Description { get; set; }
    public string? CoverImage { get; set; }
}
