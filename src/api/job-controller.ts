import express, { Request, Response, Router } from 'express';
import { Jobs } from '../models/interfaces/jobs';
import { JobQueueService } from '../services/job-queue.service';

export class JobController {
  public router: Router;

  constructor(private jobQueueService: JobQueueService) {
    this.router = express.Router();
    this.initRoutes();
  }

  private initRoutes(): void {
    this.router.get('/:id', this.getJobById);
    this.router.post('/', this.createJob);
  }

  private getJobById = async (req: Request, res: Response): Promise<void> => {
    try {
      const jobId = parseInt(req.params.id);
      const jobs = this.jobQueueService.getJobByIdWithMappings(jobId);

      if ((await jobs).job == null) {
        res.status(404).json({
          message: 'Job not found',
        });
      }

      res.json({
        message: 'Job with Mappings',
        data: jobs,
      });
    } catch {
      res.status(404).json({
        message: 'Error',
      });
    }
  };

  private createJob = async (req: Request, res: Response): Promise<void> => {
    try {
      const job = await this.jobQueueService.createJob(req.body);

      res.status(201).json(job);
    } catch (err: any) {
      res.status(400).json({ error: err.message });
    }
  };
}
