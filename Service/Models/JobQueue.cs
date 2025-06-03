namespace Service.Models;

public class JobQueue
{
    public int Id { get; set; }
    public int? MangaId { get; set; }
    public bool Completed { get; set; }
    public string Type { get; set; } = string.Empty;
    public int Attempts { get; set; }
    public int MaxAttempts { get; set; }
    public DateTime? LastAttemptAt { get; set; }
    public DateTime? CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public DateTime? ScheduledAt { get; set; }
    public DateTime? CompletedAt { get; set; }
    public string? Error { get; set; }
}
