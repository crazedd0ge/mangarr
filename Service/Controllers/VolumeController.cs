using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/Volume")]
public class VolumeController : ControllerBase
{
    private readonly IVolumeService _volumeService;

    public VolumeController(IVolumeService volumeService)
    {
        _volumeService = volumeService;
    }
}
