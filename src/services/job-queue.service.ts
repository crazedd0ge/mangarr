import { JobMapping, JobQueue } from '../db/schema';
import { RequestJob } from '../models';
import { Jobs } from '../models/interfaces/jobs';
import { JobQueueWorker } from '../workers';

export class JobQueueService {
  constructor(private jobQueueWorker: JobQueueWorker) {}

  public async createJob(job: RequestJob): Promise<Jobs> {
    return await this.jobQueueWorker.createJobWithMappings(job);
  }

  public async getJobByIdWithMappings(jobId: number): Promise<Jobs> {
    return await this.jobQueueWorker.getJobByIdWithMappings(jobId);
  }

  public async getUnprocessedJobs(): Promise<Jobs[]> {
    return await this.jobQueueWorker.getNewJobsWithMappings();
  }
}
