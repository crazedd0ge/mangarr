import { NewChapter, NewManga, NewVolume } from '../../db/schema';

export interface RequestMangaComplete {
  manga: NewManga;
  volumesWithChapters: Array<{
    volume: Omit<NewVolume, 'id'>;
    chapters: Array<Omit<NewChapter, 'mangaId' | 'volumeId'>>;
  }>;
  createJobQueueItem?: boolean;
}
