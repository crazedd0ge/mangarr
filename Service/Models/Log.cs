namespace Service.Models;

public class Log
{
    public long id { get; set; }
    public DateTime CreatedAt { get; set; }
    public bool IsError { get; set; }
    public bool IsWarning { get; set; }
    public bool IsInfo { get; set; }
    public string? Message { get; set; }
}
