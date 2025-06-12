
using System.Collections.Generic;
using Service.Models.Requests;
using Service.Models.Response;

public interface IMangaService
{
    // Partial Manga Functions
    Task<IEnumerable<Manga>?> GetAllPartialManga();
    Task<Manga?> GetPartialMangaById(long id);
    Task<IEnumerable<Manga>?> GetParitalMangaByName(string name);
    Task<Manga?> CreateParitalManga(MangaRequest data);
    Task<Manga?> UpdatePartialManga(MangaUpdateRequest daya);

    // Whole Manga Functions



}
