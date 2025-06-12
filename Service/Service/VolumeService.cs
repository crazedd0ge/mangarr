public class VolumeService : IVolumeService
{
    private readonly IVolumeWorker _volumeWorker;

    public VolumeService(IVolumeWorker volumeWorker)
    {
        _volumeWorker = volumeWorker;
    }

    public async Task<Volume?> GetVolumeById(long id)
    {
        var response = await _volumeWorker.GetVolumeById(id);

        return response;
    }

    public async Task<IEnumerable<Volume>?> GetVolumesByManga(long mangaId)
    {
        var response = await _volumeWorker.GetVolumesByManga(mangaId);

        return response;
    }

    public async Task<Volume?> CreateVolume(VolumeRequest data)
    {
        var response = await _volumeWorker.CreateVolume(data);

        return response;
    }

    public async Task<Volume?> UpdateVolume(VolumeUpdateRequest data)
    {
        var response = await _volumeWorker.UpdateVolume(data);

        return response;
    }

    public async Task<long?> DeleteVolume(long id)
    {
        var response = await _volumeWorker.DeleteVolume(id);

        return response;
    }
}
