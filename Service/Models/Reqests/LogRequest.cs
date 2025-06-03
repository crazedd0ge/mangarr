namespace Service.Models.Reqests;

public class LogRequest
{
    public bool IsError { get; set; }
    public bool IsWarning { get; set; }
    public bool IsInfo { get; set; }
    public string? Message { get; set; }
}
