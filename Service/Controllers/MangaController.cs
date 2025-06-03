using System.Data;
using Dapper;
using Microsoft.AspNetCore.Mvc;

namespace Service.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MangaController : ControllerBase
{
    private readonly IDbConnection _connection;
    private readonly IMangaWorker _mangaWorker;

    public MangaController(IDbConnection connection, IMangaWorker mangaWorker)
    {
        _connection = connection;
        _mangaWorker = mangaWorker;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Manga>>> GetAllManga()
    {
        var manga = await _mangaWorker.GetManga();
        return Ok(manga);
    }
}
