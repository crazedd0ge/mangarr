import { Manga } from '../../db';
import { VolumeChapters } from './volume-chapters';

export interface MangaComplete {
  manga: Manga;
  volumes: VolumeChapters[];
}
