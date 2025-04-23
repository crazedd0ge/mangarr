import { NewManga, NewVolume } from "src/db/schema";

export interface RequestMangaVolumes {
  manga: NewManga;
  volumes: Array<Omit<NewVolume, 'id'>>;
}
