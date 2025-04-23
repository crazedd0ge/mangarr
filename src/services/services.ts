import { ChapterWorker, JobQueueWorker, LogWorker, MangaWorker, VolumeWorker } from '../workers';
import { JobQueueService, LoggerService, MangaService } from '.';
import { JobController, MangaController } from '../api';

export class ServiceContainer {
  // Workers (lowest level dependencies)
  public logWorker = new LogWorker();
  public volumeWorker = new VolumeWorker();
  public chapterWorker = new ChapterWorker();
  public jobQueueWorker = new JobQueueWorker();
  public mangaWorker = new MangaWorker();

  // Services (mid-level dependencies)
  public loggerService = new LoggerService(this.logWorker);
  public jobQueueService = new JobQueueService(this.jobQueueWorker);
  public mangaService = new MangaService(this.mangaWorker, this.loggerService, this.volumeWorker, this.chapterWorker, this.jobQueueService);

  // Controllers (highest level)
  public mangaController = new MangaController(this.mangaWorker, this.mangaService);
  public jobController = new JobController(this.jobQueueService);
}
