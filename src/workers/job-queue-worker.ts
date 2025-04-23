import { eq, and } from 'drizzle-orm';
import { jobMapping, JobMapping, JobQueue, jobQueue, NewJobMapping, NewJobQueue } from '../db/index';
import { RequestJob } from '../models';
import { db } from '../db/index';
import { Jobs } from '../models/interfaces/jobs';

export class JobQueueWorker {
  public async createJob(data: Omit<NewJobQueue, 'id'>): Promise<JobQueue> {
    const [response] = await db.insert(jobQueue).values(data).returning();

    return response;
  }

  public async createJobMapping(data: Omit<NewJobMapping, 'id'> & { jobId: number }): Promise<JobMapping> {
    const [response] = await db.insert(jobMapping).values(data).returning();

    return response;
  }

  public async createJobWithMappings(data: RequestJob): Promise<Jobs> {
    return await db.transaction(async tx => {
      const [newJob] = await tx.insert(jobQueue).values(data.job).returning();

      const createdMappings: JobMapping[] = [];
      if (data.jobMappings.length > 0) {
        const mappingWithJobId = data.jobMappings.map(mapping => ({
          ...mapping,
          jobId: newJob.id,
        }));

        const insertedMappings = await tx.insert(jobMapping).values(mappingWithJobId).returning();
        createdMappings.push(...insertedMappings);
      }

      return { job: newJob, mappings: createdMappings };
    });
  }

  public async getNewJobsWithMappings(): Promise<Jobs[]> {
    return await db.transaction(async tx => {
      const jobResponses: Array<JobQueue> = await tx
        .select()
        .from(jobQueue)
        .where(and(eq(jobQueue.completed, false), eq(jobQueue.attempts, 0)));

      const jobs: Jobs[] = [];
      if (jobResponses.length > 0) {
        jobResponses.forEach(async job => {
          const mappings: Array<JobMapping> = await tx.select().from(jobMapping).where(eq(jobMapping.jobId, job.id));

          jobs.push({ job: job, mappings: mappings });
        });
      }

      return jobs;
    });
  }

  public async getJobByIdWithMappings(jobId: number): Promise<Jobs> {
    return await db.transaction(async tx => {
      const [job] = await tx.select().from(jobQueue).where(eq(jobQueue.id, jobId));
      const jobMappings = await tx.select().from(jobMapping).where(eq(jobMapping.jobId, jobId));

      return { job: job, mappings: jobMappings };
    });
  }

  public async getAllJobs(): Promise<JobQueue[]> {
    return await db.select().from(jobQueue);
  }

  public async getJobById(id: number): Promise<JobQueue> {
    const [response] = await db.select().from(jobQueue).where(eq(jobQueue.id, id));

    return response;
  }

  public async getJobMappingsForJob(jobQueueId: number): Promise<JobMapping[]> {
    const response = await db.select().from(jobMapping).where(eq(jobMapping.jobId, jobQueueId));

    return response;
  }
}
