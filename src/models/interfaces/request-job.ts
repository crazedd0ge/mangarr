import { NewJobMapping, NewJobQueue } from '../../db/schema';

export interface RequestJob {
  job: NewJobQueue;
  jobMappings: NewJobMapping[];
}
