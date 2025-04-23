export class MetadataWorker {
  public generateChapterInfo(chapterName: string, volumeNumber: string, mangaTitle: string): string {
    const chapterNumber = chapterName.match(/\d+/)?.[0] || '0';

    return `<?xml version="1.0" encoding="utf-8"?>
            <ComicInfo xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema">
            <Series>${mangaTitle}</Series>
            <Number>${chapterNumber}</Number>
            <Volume>${volumeNumber}</Volume>
            <Title>${chapterName}</Title>
            <Summary>Chapter ${chapterNumber} of ${mangaTitle}</Summary>
            <Genre>Manga</Genre>
            </ComicInfo>`;
  }

  public createChapterOnlyComicInfo(chapterName: string, mangaTitle: string): string {
    const chapterNumber = chapterName.match(/\d+/)?.[0] || '0';

    return `<?xml version="1.0" encoding="utf-8"?>
        <ComicInfo xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema">
            <Series>${mangaTitle}</Series>
            <Number>${chapterNumber}</Number>
            <Title>${chapterName}</Title>
            <Summary>Chapter ${chapterNumber} of ${mangaTitle}</Summary>
            <Genre>Manga</Genre>
            <Language>en</Language>
        </ComicInfo>`;
  }

  public createVolumeComicInfo(mangaTitle: string, volumeNumber: string): string {
    return `<?xml version="1.0" encoding="utf-8"?>
        <ComicInfo xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema">
            <Series>${mangaTitle}</Series>
            <Volume>${volumeNumber}</Volume>
            <Title>Volume ${volumeNumber}</Title>
            <Summary>Volume ${volumeNumber} of ${mangaTitle}</Summary>
            <Genre>Manga</Genre>
            <Language>en</Language>
        </ComicInfo>`;
  }
}
