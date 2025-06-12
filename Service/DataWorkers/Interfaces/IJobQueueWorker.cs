public interface IJobQueueWorker
{
    Task<IEnumerable<JobQueue>> GetJobs();
    Task<IEnumerable<JobMapping>> GetJobMappings(long jobId);
    Task<JobQueue> CreateJobQueue(JobQueueRequest data);
    Task<JobMapping> CreateJobMapping(JobMappingRequest data);
    Task<JobQueue> UpdateJobQueue(JobQueueUpdateRequest data);
    Task<JobMapping> UpdateJobMapping(JobMappingUpdateRequest data);
}
