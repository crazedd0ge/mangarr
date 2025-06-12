
using System.Text.RegularExpressions;

public class MetadataService : IMetadataService
{
    public async Task<string> GenerateVolumeXML(string title, string volumeNumber)
    {
        return $@"
            <?xml version=""1.0"" encoding=""utf-8""?>
                <ComicInfo xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"">
                <Series>{title}</Series>
                <Volume>{volumeNumber}</Volume>
                <Title>Volume {volumeNumber}</Title>
                <Summary>Volume {volumeNumber} of {title}</Summary>
                <Genre>Manga</Genre>
                <Language>en</Language>
            </ComicInfo>";
    }

    public async Task<string> GenerateChapterXML(string title, string chapterName, string volumeNumber)
    {
        var chapterNumberMatch = Regex.Match(chapterName, @"\d+");
        var chapterNumber = chapterNumberMatch.Success ? chapterNumberMatch.Value : "0";

        return $@"
            <?xml version=""1.0"" encoding=""utf-8""?>
                <ComicInfo xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"">
                <Series>{title}</Series>
                <Number>{chapterNumber}</Number>
                <Volume>{volumeNumber}</Volume>
                <Title>{chapterName}</Title>
                <Summary>Chapter ${chapterNumber} of {title}</Summary>
                <Genre>Manga</Genre>
            </ComicInfo>";
    }


}
