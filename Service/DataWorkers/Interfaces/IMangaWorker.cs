using Service.Models.Requests;

public interface IMangaWorker
{
    Task<IEnumerable<Manga>> GetManga();
    Task<Manga?> GetMangaById(long id);
    Task<IEnumerable<Manga>?> GetMangaByName(string name);
    Task<Manga?> CreateManga(MangaRequest data);
    Task<Manga?> UpdateManga(MangaUpdateRequest data);
    Task<long?> DeleteManga(long id);

}
