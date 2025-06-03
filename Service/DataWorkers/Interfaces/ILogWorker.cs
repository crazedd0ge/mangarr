using Service.Models;
using Service.Models.Reqests;

public interface ILogWorker
{
    Task<IEnumerable<Log>> GetErrorLogs(DateTime stateTime, DateTime endTime, string? searchTerm);
    Task<IEnumerable<Log>> GetWarningLogs(DateTime stateTime, DateTime endTime, string? searchTerm);
    Task<long> CreateLog(LogRequest data);

}
