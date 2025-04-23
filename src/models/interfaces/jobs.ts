import { JobMapping, JobQueue } from '../../db/schema';

export interface Jobs {
  job: JobQueue;
  mappings: JobMapping[];
}
