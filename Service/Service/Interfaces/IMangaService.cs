
using System.Collections.Generic;
using Service.Models.Reqests;
using Service.Models.Response;

public interface IMangaService
{
    // Task<IEnumerable<MangaResponse>?> GetMangaFull(string? name, int? id);

    Task<IEnumerable<Manga>?> GetAllPartialManga();
    Task<Manga?> GetPartialMangaById(long id);
    Task<IEnumerable<Manga>?> GetParitalMangaByName(string name);
    Task<Manga?> CreateParitalManga(MangaRequest data);
    Task<Manga?> UpdatePartialManga(MangaUpdateRequest daya);
}
