using System.Data;
using Service.Models;
using Service.Models.Reqests;

namespace Service.DataWorkers;

public class VolumeWorker : IVolumeWorker
{
    private readonly IDbConnection _connection;

    public VolumeWorker(IDbConnection connection)
    {
        _connection = connection;
    }

    public async Task<Volume> CreateVolume(VolumeRequest data)
    {
        throw new NotImplementedException();
    }

    public async Task<bool> DeleteVolume(long id)
    {
        throw new NotImplementedException();
    }

    public async Task<Volume> GetVolumeById(long id)
    {
        throw new NotImplementedException();
    }

    public async Task<IEnumerable<Volume>> GetVolumesByManga(long mangaId)
    {
        throw new NotImplementedException();
    }

    public async Task<Volume> UpdateVolume(VolumeUpdateRequest data)
    {
        throw new NotImplementedException();
    }
}
