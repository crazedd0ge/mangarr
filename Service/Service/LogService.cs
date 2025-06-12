public class LogService : ILogService
{
    private readonly ILogWorker _logWorker;

    public LogService(ILogWorker logWorker)
    {
        _logWorker = logWorker;
    }

    public async Task<IEnumerable<Log>> GetErrorLogs(DateTime stateTime, DateTime endTime, string? searchTerm)
    {
        var response = await _logWorker.GetErrorLogs(stateTime, endTime, searchTerm);

        return response;
    }

    public async Task<IEnumerable<Log>> GetWarningLogs(DateTime stateTime, DateTime endTime, string? searchTerm)
    {
        var response = await _logWorker.GetWarningLogs(stateTime, endTime, searchTerm);

        return response;
    }

    public async Task<long> CreateLog(LogRequest data)
    {
        var response = await _logWorker.CreateLog(data);

        return response;
    }
}
