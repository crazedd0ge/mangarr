import { eq } from 'drizzle-orm';
// import { Chapter, chapters, NewChapter } from '';
import { Chapter, chapters, db, NewChapter } from '../db/index';

export class ChapterWorker {
  public async createChapter(data: NewChapter): Promise<Chapter[]> {
    return await db.insert(chapters).values(data).returning();
  }

  public async getChaptersByMangaId(mangaId: number): Promise<Chapter[]> {
    return await db.select().from(chapters).where(eq(chapters.mangaId, mangaId)).orderBy(chapters.chapterNumber);
  }

  public async getChaptersByVolumeId(volumeId: number): Promise<Chapter[]> {
    return await db.select().from(chapters).where(eq(chapters.volumeId, volumeId)).orderBy(chapters.chapterNumber);
  }

  public async updateChapter(id: number, data: Partial<Omit<NewChapter, 'id'>>): Promise<Chapter | undefined> {
    const [updatedChapter] = await db
      .update(chapters)
      .set({ ...data, updatedAt: new Date() })
      .where(eq(chapters.id, id))
      .returning();
    return updatedChapter;
  }

  public async markChapterAsDownloaded(id: number): Promise<Chapter | undefined> {
    const [updatedChapter] = await db.update(chapters).set({ downloaded: true, updatedAt: new Date() }).where(eq(chapters.id, id)).returning();
    return updatedChapter;
  }

  public async updateChapterUrl(chapterId: number, chapterUrl: string): Promise<Chapter | undefined> {
    return await this.updateChapter(chapterId, { url: chapterUrl });
  }
}
