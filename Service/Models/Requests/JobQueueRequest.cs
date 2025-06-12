namespace Service.Models.Requests;

public class JobQueueRequest
{
    public int? MangaId { get; set; }
    public string Type { get; set; } = string.Empty;
    public int MaxAttempts { get; set; } = 3;
    public DateTime? ScheduledAt { get; set; }
}
