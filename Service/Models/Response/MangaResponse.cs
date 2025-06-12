using Service.Models;

public class MangaResponse
{
    public required Manga Manga { get; set; }
    public required IEnumerable<VolumeMetadata> VolumeMetadata { get; set; }
}

public class VolumeMetadata
{
    public required Volume Volume { get; set; }
    public required IEnumerable<Chapter> Chapters { get; set; }
}
