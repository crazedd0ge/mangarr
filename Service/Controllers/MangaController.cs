using Microsoft.AspNetCore.Mvc;
[ApiController]
[Route("api/Manga")]
public class MangaController : ControllerBase
{
    public IMangaService _mangaService { get; }

    public MangaController(IMangaService mangaService)
    {
        _mangaService = mangaService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Manga>>> GetAllManga()
    {
        var manga = await _mangaService.GetAllManga();

        return Ok(manga);
    }

    [HttpGet]
    [Route("id/{id}")]
    public async Task<ActionResult<Manga?>> GetMangaById([FromRoute] long id)
    {
        var manga = await _mangaService.GetMangaById(id);

        return Ok(manga);
    }

    [HttpGet]
    [Route("name/{name}")]
    public async Task<ActionResult<IEnumerable<Manga>?>> GetMangaByName([FromRoute] string name)
    {
        var manga = await _mangaService.GetParitalMangaByName(name);

        return Ok(manga);
    }

    [HttpPost]
    [Route("Create")]
    public async Task<ActionResult<Manga?>> CreateManga([FromBody] MangaRequest data)
    {
        var manga = await _mangaService.CreateParitalManga(data);

        return manga;
    }

    [HttpPost]
    [Route("Update")]
    public async Task<ActionResult<Manga?>> UpdateManga([FromBody] MangaUpdateRequest data)
    {
        var manga = await _mangaService.UpdateManga(data);

        return manga;
    }

}
