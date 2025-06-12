public class MangaService : IMangaService
{
    public IMangaWorker _mangaWorker { get; }

    public MangaService(IMangaWorker mangaWorker)
    {
        _mangaWorker = mangaWorker;
    }

    public async Task<IEnumerable<Manga>?> GetAllManga()
    {
        var response = await _mangaWorker.GetManga();

        return response;
    }
    public async Task<Manga?> GetMangaById(long id)
    {
        var response = await _mangaWorker.GetMangaById(id);

        return response;
    }

    public async Task<IEnumerable<Manga>?> GetParitalMangaByName(string name)
    {
        var response = await _mangaWorker.GetMangaByName(name);

        return response;
    }

    public async Task<Manga?> CreateParitalManga(MangaRequest data)
    {
        var response = await _mangaWorker.CreateManga(data);

        return response;
    }

    public async Task<Manga?> UpdateManga(MangaUpdateRequest data)
    {
        var response = await _mangaWorker.UpdateManga(data);

        return response;
    }
}
