namespace Service.Models.Reqests;

public class JobMappingUpdateRequest
{
    public int Id { get; set; }
    public double? VolumeNumber { get; set; }
    public string? ChapterKeyword { get; set; }
    public int? ChapterNumber { get; set; }
    public bool? Completed { get; set; }
    public DateTime? CompletedAt { get; set; }
    public DateTime? CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}
