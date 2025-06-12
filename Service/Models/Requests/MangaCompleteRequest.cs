namespace Service.Models.Requests;

public class MangaCompleteRequest
{
    public required MangaRequest Manga { get; set; }
    public required IEnumerable<VolumeData> VolumeMappings { get; set; }
}

public class VolumeData
{
    public required VolumeRequest Volume { get; set; }
    public required IEnumerable<ChapterRequest> Chapters { get; set; }
}
