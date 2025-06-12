using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/Log")]
public class LogController : ControllerBase
{
    private readonly ILogService _logService;

    public LogController(ILogService logService)
    {
        _logService = logService;
    }
}
