using System.Data;
using Dapper;
using Microsoft.AspNetCore.Mvc;
using Service.Models.Reqests;

namespace Service.Controllers;

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
        var manga = await _mangaService.GetAllPartialManga();

        return Ok(manga);
    }

    [HttpGet]
    [Route("id/{id}")]
    public async Task<ActionResult<Manga?>> GetMangaById([FromRoute] long id)
    {
        var manga = await _mangaService.GetPartialMangaById(id);

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
    public async Task<ActionResult<Manga?>> CreatePartialManga([FromBody] MangaRequest data)
    {
        var manga = await _mangaService.CreateParitalManga(data);

        return manga;
    }

    [HttpPost]
    [Route("Update")]
    public async Task<ActionResult<Manga?>> UpdatePartialManga([FromBody] MangaUpdateRequest data)
    {
        var manga = await _mangaService.UpdatePartialManga(data);

        return manga;
    }

}
