namespace Service.Models.Reqests;

public class MangaRequest
{
    public int Id { get; set; }
    public double? VolumeNumber { get; set; }
    public string? Title { get; set; }
    public string? Url { get; set; }
    public string? Description { get; set; }
    public string? CoverImage { get; set; }
    public bool? Complete { get; set; }
}
