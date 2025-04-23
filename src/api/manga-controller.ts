import express, { Request, Response, Router } from 'express';
import { NewManga } from '../db';
import { RequestMangaComplete } from '../models';
import { MangaService } from '../services/manga.service';
import { MangaWorker } from '../workers';

export class MangaController {
  public router: Router;

  constructor(
    private mangaWorker: MangaWorker,
    private mangaService: MangaService
  ) {
    this.router = express.Router();
    this.initRoutes();
  }

  private initRoutes(): void {
    this.router.get('/', this.getManga);
    this.router.get('/:id', this.getCompleteMangaStructure);
    this.router.get('/:name', this.getMangaByName);
    this.router.post('/', this.createManga);
    this.router.post('/complete', this.createCompleteManga);
  }

  private getManga = async (req: Request, res: Response): Promise<void> => {
    try {
      const manga = await this.mangaWorker.getAllManga();

      res.json({
        data: manga,
      });
    } catch (err: any) {
      res.status(404).json({
        message: 'Error',
        data: err,
      });
    }
  };

  private getMangaById = async (req: Request, res: Response): Promise<void> => {
    try {
      const mangaId = parseInt(req.params.id);
      const manga = await this.mangaWorker.getMangaById(mangaId);

      res.json({
        data: manga,
      });
    } catch (err: any) {
      res.status(404).json({
        message: 'Error',
        data: err,
      });
    }
  };

  private getMangaByName = async (req: Request, res: Response): Promise<void> => {
    try {
      const mangaName = req.params.name;
      const manga = await this.mangaWorker.getMangaByName(mangaName);

      res.json({
        data: manga,
      });
    } catch (err: any) {
      res.status(404).json({
        message: 'Error',
        data: err,
      });
    }
  };

  private getCompleteMangaStructure = async (req: Request, res: Response): Promise<void> => {
    try {
      const mangaId = parseInt(req.params.id);
      const manga = await this.mangaService.buildFullMangaResponse(mangaId);

      res.json({
        data: manga,
      });
    } catch (err: any) {
      res.status(404).json({
        message: 'Error',
        data: err,
      });
    }
  };

  private createCompleteManga = async (req: Request<{}, {}, RequestMangaComplete>, res: Response) => {
    try {
      const response = this.mangaService.createManga(req.body);

      res.status(201).json(response);
    } catch (err: any) {
      res.status(400).json({ error: err.message });
    }
  };

  private createManga = async (req: Request<{}, {}, NewManga>, res: Response): Promise<void> => {
    try {
      const manga = await this.mangaWorker.createManga(req.body);

      res.status(201).json(manga);
    } catch (err: any) {
      res.status(400).json({ error: err.message });
    }
  };
}
