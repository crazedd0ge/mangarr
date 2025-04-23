import { Chapter, NewJobMapping, NewJobQueue, Volume } from '../db';
import { MangaComplete, RequestJob, RequestMangaComplete, VolumeChapters } from '../models';
import { ChapterWorker, MangaWorker, VolumeWorker } from '../workers';
import { JobQueueService } from './job-queue.service';
import { LoggerService } from './logger.service';

export class MangaService {
  constructor(
    private mangaWorker: MangaWorker,
    private loggerService: LoggerService,
    private volumeWorker: VolumeWorker,
    private chapterWorker: ChapterWorker,
    private jobQueueService: JobQueueService
  ) {}

  public async createManga(data: RequestMangaComplete): Promise<MangaComplete | null> {
    this.loggerService.logInfo(
      `Request to create manga with name of ${data.manga.title} with ${data.volumesWithChapters.length} volumes and ${data.volumesWithChapters.map(x => x.chapters.length)}`
    );

    try {
      let manga: MangaComplete;

      try {
        manga = await this.mangaWorker.createCompleteStructure(data);
        this.loggerService.logInfo(`Created manga with id: ${manga.manga.id}`);
      } catch (err: any) {
        this.loggerService.logError(`Failed to create manga with name of ${data.manga.title}`, err);
        throw new err(err);
      }

      if (data.createJobQueueItem) {
        this.loggerService.logInfo(`Creating job queue item for mangaId: ${manga.manga.id}`);

        try {
          let request: RequestJob;
          const jobQueueItem: NewJobQueue = {
            type: 'download_manga',
            mangaId: manga.manga.id,
          };
          let jobMappingItems: NewJobMapping[] = [];

          if (manga.volumes == null || manga.volumes.length == 0) {
            this.loggerService.logWarning(`No volumes exist for the request manga`);

            return null;
          }

          if (manga.volumes != null && manga.volumes.length !== 0) {
            const anyChapters = manga.volumes.some(x => x.chapters);

            if (!anyChapters) {
              this.loggerService.logWarning(`No chapters exist for the request manga`);

              return null;
            }

            manga.volumes.forEach(volume => {
              if (volume.chapters == null || volume.chapters.length == 0) {
                this.loggerService.logWarning(`No chapters exist for volume ID:${volume.volume?.id} in manga ID:${manga.manga.id}`);

                return;
              }

              const jobMappingsForChapters: NewJobMapping[] = volume.chapters.map(chapter => {
                return {
                  volumeNumber: volume.volume?.volumeNumber,
                  chapterKeyword: chapter.chapterKeyword,
                  chapterNumber: chapter.chapterNumber,
                };
              });

              jobMappingItems.push(...jobMappingsForChapters);
            });
          }

          request = { job: jobQueueItem, jobMappings: jobMappingItems };

          await this.jobQueueService.createJob(request);

          this.loggerService.logInfo(`Created job queue item for manga ID: ${manga.manga.id}`);
        } catch (err: any) {
          this.loggerService.logError(`Failed to create job queue item for manga ID: ${manga.manga.id}`, err);
          throw new err(err);
        }
      }

      return manga;
    } catch (err: any) {
      this.loggerService.logError(`Failed to create manga ${data.manga.title}`, err);
      throw new err(err);
    }
  }

  public async buildFullMangaResponse(id: number): Promise<MangaComplete | null> {
    if (id == null) {
      this.loggerService.logWarning('No MangaID provided');

      return null;
    }

    const manga = await this.mangaWorker.getMangaById(id);
    const volumes = (await this.volumeWorker.getVolumesByMangaId(id)).sort((a: Volume, b: Volume) => a.volumeNumber - b.volumeNumber);
    const chapters = await this.chapterWorker.getChaptersByMangaId(id);

    const mappedVolumes: VolumeChapters[] = [];

    if (volumes.length > 0) {
      const chaptersMappedToVolumes: VolumeChapters[] = volumes.map(volume => ({
        volume: volume,
        chapters: chapters.filter(x => (x.volumeId = volume.id)).sort((a: Chapter, b: Chapter) => a.chapterNumber - b.chapterNumber),
      }));

      mappedVolumes.push(...chaptersMappedToVolumes);
    }

    let response: MangaComplete = {
      manga: manga,
      volumes: mappedVolumes,
    };

    return response;
  }


  private async doesMangaExists(): Promise<boolean> {

  }

}
