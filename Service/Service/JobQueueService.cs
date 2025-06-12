public class JobQueueService : IJobQueueService
{
    private readonly IJobQueueWorker _jobQueueWorker;

    public JobQueueService(IJobQueueWorker jobQueueWorker)
    {
        _jobQueueWorker = jobQueueWorker;
    }

    public async Task<IEnumerable<JobMapping>> GetJobMappings(long jobId)
    {
        var response = await _jobQueueWorker.GetJobMappings(jobId);

        return response;
    }

    public async Task<IEnumerable<JobQueue>> GetJobs()
    {
        var response = await _jobQueueWorker.GetJobs();

        return response;
    }

    public async Task<JobMapping> CreateJobMapping(JobMappingRequest data)
    {
        var response = await _jobQueueWorker.CreateJobMapping(data);

        return response;
    }

    public async Task<JobQueue> CreateJobQueue(JobQueueRequest data)
    {
        var response = await _jobQueueWorker.CreateJobQueue(data);

        return response;
    }

    public async Task<JobMapping> UpdateJobMapping(JobMappingUpdateRequest data)
    {
        var response = await _jobQueueWorker.UpdateJobMapping(data);

        return response;
    }

    public async Task<JobQueue> UpdateJobQueue(JobQueueUpdateRequest data)
    {
        var response = await _jobQueueWorker.UpdateJobQueue(data);

        return response;
    }
}
