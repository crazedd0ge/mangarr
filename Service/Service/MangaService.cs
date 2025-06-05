

using Service.Models.Reqests;

public class MangaService : IMangaService
{
    public IMangaWorker _mangaWorker { get; }

    public MangaService(IMangaWorker mangaWorker)
    {
        _mangaWorker = mangaWorker;
    }

    public async Task<IEnumerable<Manga>?> GetAllPartialManga()
    {
        return await _mangaWorker.GetManga();
    }
    public async Task<Manga?> GetPartialMangaById(long id)
    {
        return await _mangaWorker.GetMangaById(id);
    }

    public async Task<IEnumerable<Manga>?> GetParitalMangaByName(string name)
    {
        return await _mangaWorker.GetMangaByName(name);
    }

    public async Task<Manga?> CreateParitalManga(MangaRequest data)
    {
        return await _mangaWorker.CreateManga(data);
    }

    public async Task<Manga?> UpdatePartialManga(MangaUpdateRequest daya)
    {
        throw new NotImplementedException();
    }
}
