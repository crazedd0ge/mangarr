import { and, eq } from 'drizzle-orm';
import { Chapter, chapters, NewChapter, NewVolume, Volume, volumes } from '../db/index';
import { db } from '../db/index';

export class VolumeWorker {
  public async createVolume(data: NewVolume): Promise<Volume[]> {
    return await db.insert(volumes).values(data).returning();
  }

  public async getVolumesByMangaId(mangaId: number): Promise<Volume[]> {
    return await db.select().from(volumes).where(eq(volumes.mangaId, mangaId)).orderBy(volumes.volumeNumber);
  }

  public async getVolumeById(volumeId: number): Promise<Volume | undefined> {
    const results = await db.select().from(volumes).where(eq(volumes.id, volumeId));
    return results[0];
  }

  public async updateVolume(id: number, data: Partial<Omit<Volume, 'id' | 'createdAt' | 'updatedAt'>>): Promise<Volume | undefined> {
    const [updatedVolume] = await db
      .update(volumes)
      .set({ ...data, updatedAt: new Date() })
      .where(eq(volumes.id, id))
      .returning();

    return updatedVolume;
  }

  public async markVolumeAsCompleted(mangaId: number, volumeId: number): Promise<Volume[]> {
    return await db
      .update(volumes)
      .set({ complete: true, updatedAt: new Date() })
      .where(and(eq(volumes.mangaId, mangaId), eq(volumes.id, volumeId)))
      .returning();
  }

  public async createVolumeWithChapters(volumeData: NewVolume, chapterData: Array<Omit<NewChapter, 'chapterId' | 'mangaId'>>): Promise<{ volume: Volume; chapters: Chapter[] }> {
    return await db.transaction(async tx => {
      const [newVolume] = await tx.insert(volumes).values(volumeData).returning();

      const createdChapters: Chapter[] = [];
      if (chapterData.length > 0) {
        const chaptersWithIds = chapterData.map(chapter => ({
          ...chapter,
          mangaId: newVolume.mangaId,
          volumeId: newVolume.id,
        }));

        const insertedChapters = await tx.insert(chapters).values(chaptersWithIds).returning();
        createdChapters.push(...insertedChapters);
      }

      return { volume: newVolume, chapters: createdChapters };
    });
  }

  public async updateVolumeUrls(
    volumeId: number,
    urlData: {
      volumeUrl?: string;
      coverImage?: string;
    }
  ): Promise<Volume | undefined> {
    const updateData: Partial<Volume> = {};

    if (urlData.volumeUrl) updateData.url = urlData.volumeUrl;
    if (urlData.coverImage) updateData.coverImage = urlData.coverImage;

    if (Object.keys(updateData).length === 0) return;

    return await this.updateVolume(volumeId, updateData);
  }
}
