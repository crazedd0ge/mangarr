using System.Data;
using Dapper;
public class VolumeWorker : IVolumeWorker
{
    private readonly IDbConnection _connection;

    public VolumeWorker(IDbConnection connection)
    {
        _connection = connection;
    }

    public async Task<Volume?> GetVolumeById(long id)
    {
        var response = (await _connection.QueryAsync<Volume?>(SqlScripts.Volumes.GetById, id)).FirstOrDefault();

        return response;
    }

    public async Task<IEnumerable<Volume>?> GetVolumesByManga(long mangaId)
    {
        var response = await _connection.QueryAsync<Volume>(SqlScripts.Volumes.GetByMangaId, mangaId);

        return response;
    }

    public async Task<Volume?> CreateVolume(VolumeRequest data)
    {
        var response = (await _connection.QueryAsync<Volume?>(SqlScripts.Volumes.Insert, new
        {
            data.MangaId,
            data.VolumeNumber,
            data.Title,
            data.Url,
            data.Description,
            data.CoverImage,
            data.Complete,
            CreatedAt = DateTime.Now,
            UpdatedAt = DateTime.Now
        })).FirstOrDefault();

        return response;
    }

    public async Task<Volume?> UpdateVolume(VolumeUpdateRequest data)
    {
        var response = (await _connection.QueryAsync<Volume?>(SqlScripts.Volumes.Update, new
        {
            data.Id,
            data.MangaId,
            data.VolumeNumber,
            data.Title,
            data.Url,
            data.Description,
            data.CoverImage,
            data.Complete,
            CreatedAt = DateTime.Now,
            UpdatedAt = DateTime.Now
        })).FirstOrDefault();

        return response;
    }

    public async Task<long?> DeleteVolume(long id)
    {
        var response = (await _connection.QueryAsync<long?>(SqlScripts.Manga.Update, id)).FirstOrDefault();

        return response;
    }
}
