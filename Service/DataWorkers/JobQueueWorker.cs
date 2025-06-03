using System.Data;
using Service.DataWorkers.Interfaces;
using Service.Models;
using Service.Models.Reqests;

namespace Service.DataWorkers;

public class JobQueueWorker : IJobQueueWorker
{
    private readonly IDbConnection _connection;

    public JobQueueWorker(IDbConnection connection)
    {
        _connection = connection;
    }

    public async Task<JobMapping> CreateJobMapping(JobMappingRequest data)
    {
        throw new NotImplementedException();
    }

    public async Task<JobQueue> CreateJobQueue(JobQueueRequest data)
    {
        throw new NotImplementedException();
    }

    public async Task<IEnumerable<JobMapping>> GetJobMappings(long jobId)
    {
        throw new NotImplementedException();
    }

    public async Task<IEnumerable<JobQueue>> GetJobs()
    {
        throw new NotImplementedException();
    }

    public async Task<JobMapping> UpdateJobMapping(JobMappingUpdateRequest data)
    {
        throw new NotImplementedException();
    }

    public async Task<JobQueue> UpdateJobQueue(JobQueueUpdateRequest data)
    {
        throw new NotImplementedException();
    }
}
