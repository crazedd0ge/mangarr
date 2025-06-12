using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/JobQueue")]
public class JobQueueController : ControllerBase
{
    private readonly IJobQueueService _jobQueueService;

    public JobQueueController(IJobQueueService jobQueueService)
    {
        _jobQueueService = jobQueueService;
    }
}
