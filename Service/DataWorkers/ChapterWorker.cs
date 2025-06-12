using System.Data;
using Dapper;
public class ChapterWorker : IChapterWorker
{
    private readonly IDbConnection _connection;
    public ChapterWorker(IDbConnection connection)
    {
        _connection = connection;
    }

    public async Task<IEnumerable<Chapter>?> GetChaptersByManga(long id)
    {
        var response = await _connection.QueryAsync<Chapter>(SqlScripts.Chapters.GetByMangaId, id);

        return response;
    }

    public async Task<IEnumerable<Chapter>?> GetChaptersByVolume(long id)
    {
        var response = await _connection.QueryAsync<Chapter>(SqlScripts.Chapters.GetByVolumeId, id);

        return response;
    }

    public async Task<Volume?> CreateChapter(ChapterRequest data)
    {
        var response = (await _connection.QueryAsync<Volume>(SqlScripts.Chapters.Insert, new
        {
            data.MangaId,
            data.VolumeId,
            data.ChapterKeyword,
            data.ChapterNumber,
            data.Title,
            data.Url,
            data.Downloaded,
            CreatedAt = DateTime.Now,
            UpdatedAt = DateTime.Now
        })).FirstOrDefault();

        return response;
    }

    public async Task<Volume?> UpdateChapter(ChapterUpdateRequest data)
    {
        var response = (await _connection.QueryAsync<Volume>(SqlScripts.Chapters.Update, new
        {
            data.Id,
            data.ChapterKeyword,
            data.ChapterNumber,
            data.Title,
            data.Url,
            data.Downloaded,
            CreatedAt = DateTime.Now,
            UpdatedAt = DateTime.Now
        })).FirstOrDefault();

        return response;
    }

    public async Task<long> DeleteChapter(long id)
    {
        return (await _connection.QueryAsync<long>(SqlScripts.Chapters.Delete, id)).FirstOrDefault();
    }



}
