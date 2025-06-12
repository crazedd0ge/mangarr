
public class ChapterService : IChapterService
{
    private readonly IChapterWorker _chapterWorker;

    public ChapterService(IChapterWorker chapterWorker)
    {
        _chapterWorker = chapterWorker;
    }

    public async Task<IEnumerable<Chapter>?> GetChaptersByManga(long id)
    {
        var response = await _chapterWorker.GetChaptersByManga(id);

        return response;
    }

    public async Task<IEnumerable<Chapter>?> GetChaptersByVolume(long id)
    {
        var response = await _chapterWorker.GetChaptersByVolume(id);

        return response;
    }


    public async Task<Volume?> CreateChapter(ChapterRequest data)
    {
        var response = await _chapterWorker.CreateChapter(data);

        return response;
    }

    public async Task<Volume?> UpdateChapter(ChapterUpdateRequest data)
    {
        var response = await _chapterWorker.UpdateChapter(data);

        return response;
    }

    public async Task<long> DeleteChapter(long id)
    {
        var response = await _chapterWorker.DeleteChapter(id);

        return response;
    }

}
