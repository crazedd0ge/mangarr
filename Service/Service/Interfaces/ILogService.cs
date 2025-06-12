public interface ILogService
{
    Task<IEnumerable<Log>> GetErrorLogs(DateTime stateTime, DateTime endTime, string? searchTerm);
    Task<IEnumerable<Log>> GetWarningLogs(DateTime stateTime, DateTime endTime, string? searchTerm);
    Task<long> CreateLog(LogRequest data);
}
