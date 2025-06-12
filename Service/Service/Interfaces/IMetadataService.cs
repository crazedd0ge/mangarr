public interface IMetadataService
{
    Task<string> GenerateVolumeXML(string title, string volumeNumber);
    Task<string> GenerateChapterXML(string title, string chapterName, string volumeNumber);
}
