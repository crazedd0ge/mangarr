import { eq, like } from 'drizzle-orm';
import { db, Chapter, chapters, manga, Manga, NewChapter, NewManga, Volume, volumes } from '../db/index';
import { RequestMangaComplete, RequestMangaVolumes, MangaComplete } from '../models';

export class MangaWorker {
  public async createManga(data: NewManga): Promise<Manga[]> {
    return await db.insert(manga).values(data).returning();
  }

  public async getAllManga(): Promise<Manga[]> {
    return await db.select().from(manga);
  }

  public async getMangaById(mangaId: number): Promise<Manga> {
    const results = await db.select().from(manga).where(eq(manga.id, mangaId));
    return results[0];
  }

  public async getMangaByName(name: string): Promise<Manga[]> {
    const results = await db.select().from(manga).where(like(manga.title, name));
    return results;
  }

  public async updateManga(id: number, data: Partial<Manga>): Promise<Manga | undefined> {
    const [updatedManga] = await db
      .update(manga)
      .set({ ...data, updatedAt: new Date() })
      .where(eq(manga.id, id))
      .returning();
    return updatedManga;
  }

  public async deleteManga(id: number): Promise<void> {
    await db.delete(manga).where(eq(manga.id, id));
  }

  public async createMangaWithVolumes(data: RequestMangaVolumes): Promise<{ manga: Manga; volumes: Volume[] }> {
    return await db.transaction(async tx => {
      const [newManga] = await tx.insert(manga).values(data.manga).returning();

      const createdVolumes: Volume[] = [];
      if (data.volumes.length > 0) {
        const volumesWithMangaId = data.volumes.map(volume => ({
          ...volume,
          mangaId: newManga.id,
        }));

        const insertedVolumes = await tx.insert(volumes).values(volumesWithMangaId).returning();
        createdVolumes.push(...insertedVolumes);
      }

      return { manga: newManga, volumes: createdVolumes };
    });
  }

  public async createMangaWithChapters(mangaData: NewManga, chapterData: Array<Omit<NewChapter, 'id'>>): Promise<{ manga: Manga; chapters: Chapter[] }> {
    return await db.transaction(async tx => {
      const [newManga] = await tx.insert(manga).values(mangaData).returning();

      const createdChapters: Chapter[] = [];
      if (chapterData.length > 0) {
        const chaptersWithMangaId = chapterData.map(chapter => ({
          ...chapter,
          mangaId: newManga.id,
        }));

        const insertedChapters = await tx.insert(chapters).values(chaptersWithMangaId).returning();
        createdChapters.push(...insertedChapters);
      }

      return { manga: newManga, chapters: createdChapters };
    });
  }

  public async updateMangaUrls(mangaId: number, urlData: { mangaUrl?: string; coverImage?: string }): Promise<Manga | undefined> {
    const updateData: Partial<Manga> = {};

    if (urlData.mangaUrl) updateData.url = urlData.mangaUrl;
    if (urlData.coverImage) updateData.coverImage = urlData.coverImage;

    if (Object.keys(updateData).length === 0) return await this.getMangaById(mangaId);

    return await this.updateManga(mangaId, updateData);
  }

  public async createCompleteStructure(data: RequestMangaComplete): Promise<MangaComplete> {
    return await db.transaction(async tx => {
      const [newManga] = await tx.insert(manga).values(data.manga).returning();

      const results: Array<{ volume: Volume; chapters: Chapter[] }> = [];

      for (const { volume: volumeData, chapters: chapterData } of data.volumesWithChapters) {
        const [newVolume] = await tx
          .insert(volumes)
          .values({
            ...volumeData,
            mangaId: newManga.id,
          })
          .returning();

        const volumeChapters: Chapter[] = [];
        if (chapterData.length > 0) {
          const chaptersWithIds = chapterData.map(chapter => ({
            ...chapter,
            mangaId: newManga.id,
            volumeId: newVolume.id,
          }));

          const insertedChapters = await tx.insert(chapters).values(chaptersWithIds).returning();
          volumeChapters.push(...insertedChapters);
        }

        results.push({ volume: newVolume, chapters: volumeChapters });
      }

      return { manga: newManga, volumes: results };
    });
  }
}
