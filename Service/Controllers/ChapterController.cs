using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/Chapter")]
public class ChapterController : ControllerBase
{
    private readonly IChapterService _chapterService;

    public ChapterController(IChapterService chapterService)
    {
        _chapterService = chapterService;
    }
}
