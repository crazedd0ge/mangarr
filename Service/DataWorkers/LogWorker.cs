using System.Data;
using Dapper;
public class LogWorker : ILogWorker
{
    private readonly IDbConnection _connection;

    public LogWorker(IDbConnection connection)
    {
        _connection = connection;
    }

    public async Task<long> CreateLog(LogRequest data)
    {
        var logId = (await _connection.QueryAsync<long>(SqlScripts.Logs.Insert, data)).FirstOrDefault();

        return logId;
    }

    public async Task<IEnumerable<Log>> GetErrorLogs(DateTime stateTime, DateTime endTime, string? searchTerm)
    {
        var logs = await _connection.QueryAsync<Log>(SqlScripts.Logs.GetErrors, new
        {
            StartDate = stateTime,
            EndDate = endTime,
            SearchTerm = searchTerm
        });

        return logs;
    }


    public async Task<IEnumerable<Log>> GetWarningLogs(DateTime stateTime, DateTime endTime, string? searchTerm)
    {
        var logs = await _connection.QueryAsync<Log>(SqlScripts.Logs.GetWarnings, new
        {
            StartDate = stateTime,
            EndDate = endTime,
            SearchTerm = searchTerm
        });

        return logs;
    }
}
