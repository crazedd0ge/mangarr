using Service.Models;
using Service.Models.Requests;

public interface IVolumeWorker
{
    Task<IEnumerable<Volume>?> GetVolumesByManga(long mangaId);
    Task<Volume?> GetVolumeById(long id);
    Task<Volume?> CreateVolume(VolumeRequest data);
    Task<Volume?> UpdateVolume(VolumeUpdateRequest data);
    Task<long?> DeleteVolume(long id);
}
