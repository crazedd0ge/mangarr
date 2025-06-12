using Service.Models.Requests;

public class MangaService : IMangaService
{
    public IMangaWorker _mangaWorker { get; }

    public MangaService(IMangaWorker mangaWorker)
    {
        _mangaWorker = mangaWorker;
    }

    public async Task<IEnumerable<Manga>?> GetAllManga()
    {
        return await _mangaWorker.GetManga();
    }
    public async Task<Manga?> GetMangaById(long id)
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

    public async Task<Manga?> UpdateManga(MangaUpdateRequest data)
    {
        return await _mangaWorker.UpdateManga(data);
    }
}
