import { Chapter, Volume } from 'src/db';

export interface VolumeChapters {
  volume: Volume | undefined;
  chapters: Chapter[] | undefined;
}
