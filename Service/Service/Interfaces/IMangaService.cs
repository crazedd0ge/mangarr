
using Service.Models.Requests;

public interface IMangaService
{
    //  Manga Functions
    Task<IEnumerable<Manga>?> GetAllManga();
    Task<Manga?> GetMangaById(long id);
    Task<IEnumerable<Manga>?> GetParitalMangaByName(string name);
    Task<Manga?> CreateParitalManga(MangaRequest data);
    Task<Manga?> UpdateManga(MangaUpdateRequest daya);

    // Whole Manga Functions



}
