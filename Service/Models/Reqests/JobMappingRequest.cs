namespace Service.Models.Reqests;

public class JobMappingRequest
{
    public int JobId { get; set; }
    public double? VolumeNumber { get; set; }
    public string ChapterKeyword { get; set; } = string.Empty;
    public int ChapterNumber { get; set; }
    public bool Completed { get; set; }
    public DateTime? CompletedAt { get; set; }
    public DateTime? CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}
