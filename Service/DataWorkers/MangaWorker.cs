using System.Data;
using Dapper;
using Service.Models.Reqests;

namespace Service.DataWorkers;

public class MangaWorker : IMangaWorker
{
    private readonly IDbConnection _connection;

    public MangaWorker(IDbConnection connection)
    {
        _connection = connection;
    }

    public async Task<IEnumerable<Manga>> GetManga()
    {
        var response = await _connection.QueryAsync<Manga>(SqlScripts.Manga.GetAll);

        return response;
    }

    public async Task<Manga?> GetMangaById(long id)
    {
        var response = (await _connection.QueryAsync<Manga>(SqlScripts.Manga.GetById, id)).FirstOrDefault();

        return response;
    }

    public async Task<IEnumerable<Manga>?> GetMangaByName(string name)
    {
        var response = await _connection.QueryAsync<Manga>(SqlScripts.Manga.GetByName, name);

        return response;
    }

    public async Task<Manga?> CreateManga(MangaRequest data)
    {
        var response = (await _connection.QueryAsync<Manga>(SqlScripts.Manga.Insert, new
        {
            data.Title,
            data.Url,
            data.Description,
            data.CoverImage,
            CreatedAt = DateTime.Now,
            UpdatedAt = DateTime.Now
        })).FirstOrDefault();

        return response;
    }

    public async Task<Manga?> UpdateManga(MangaUpdateRequest data)
    {
        var response = (await _connection.QueryAsync<Manga>(SqlScripts.Manga.Update, new
        {
            data.Id,
            data.Title,
            data.Url,
            data.Description,
            data.CoverImage,
            UpdatedAt = DateTime.Now
        })).FirstOrDefault();

        return response;
    }

    public async Task<long?> DeleteManga(long id)
    {
        var response = (await _connection.QueryAsync<long?>(SqlScripts.Manga.Update, id)).FirstOrDefault();

        return response;
    }


}
