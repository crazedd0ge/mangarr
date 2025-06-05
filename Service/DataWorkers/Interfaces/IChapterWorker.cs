using Service.Models;
using Service.Models.Reqests;

public interface IChapterWorker
{
    Task<IEnumerable<Chapter>?> GetChaptersByManga(long id);
    Task<IEnumerable<Chapter>?> GetChaptersByVolume(long id);
    Task<Volume?> CreateChapter(ChapterRequest data);
    Task<Volume?> UpdateChapter(ChapterUpdateRequest data);
    Task<long> DeleteChapter(long id);
}
