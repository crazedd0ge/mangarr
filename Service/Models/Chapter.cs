public class Chapter
{
    public int Id { get; set; }
    public int MangaId { get; set; }
    public int? VolumeId { get; set; }
    public string ChapterKeyword { get; set; } = string.Empty;
    public double ChapterNumber { get; set; }
    public string? Title { get; set; }
    public string Url { get; set; } = string.Empty;
    public bool Downloaded { get; set; }
    public DateTime? CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}
