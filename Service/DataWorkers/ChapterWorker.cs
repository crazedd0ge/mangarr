using System.Data;
using Service.Models;
using Service.Models.Reqests;

namespace Service.DataWorkers;

public class ChapterWorker : IChapterWorker
{
    private readonly IDbConnection _connection;

    public ChapterWorker(IDbConnection connection)
    {
        _connection = connection;
    }

    public async Task<Volume> CreateChapter(ChapterRequest data)
    {
        throw new NotImplementedException();
    }

    public async Task<long> DeleteChapter(long id)
    {
        throw new NotImplementedException();
    }

    public async Task<IEnumerable<Chapter>> GetChaptersByManga(long id)
    {
        throw new NotImplementedException();
    }

    public async Task<IEnumerable<Chapter>> GetChaptersByVolume(long id)
    {
        throw new NotImplementedException();
    }

    public async Task<Volume> UpdateChapter(ChapterUpdateRequest data)
    {
        throw new NotImplementedException();
    }
}
