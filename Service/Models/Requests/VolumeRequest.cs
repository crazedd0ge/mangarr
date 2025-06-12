public class VolumeRequest
{
    public int MangaId { get; set; }
    public double VolumeNumber { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Url { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? CoverImage { get; set; }
    public bool Complete { get; set; } = false;
}
