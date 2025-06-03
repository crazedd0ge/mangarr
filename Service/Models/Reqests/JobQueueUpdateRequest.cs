namespace Service.Models.Reqests;

public class JobQueueUpdateRequest
{
    public int Id { get; set; }
    public bool Completed { get; set; }
    public DateTime? ScheduledAt { get; set; }
    public DateTime? CompletedAt { get; set; }
    public string? Error { get; set; }
    public string? Type { get; set; }
    public int? MaxAttempts { get; set; }
}
