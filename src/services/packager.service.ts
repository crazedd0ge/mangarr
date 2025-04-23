import archiver from 'archiver';
import { createWriteStream } from 'fs';
import path, { resolve } from 'path';
import { Readable } from 'stream';
import * as yauzl from 'yauzl';
import { LoggerService } from './logger.service';

interface ExtractOptions {
  allowedExtensions?: string[];
  skipFiles?: string[];
}

interface Logger {
  logInfo: (message: string) => Promise<void>;
  logError: (message: string, error: Error) => Promise<void>;
}

const DEFAULT_OPTIONS: ExtractOptions = {
  allowedExtensions: ['.jpg', '.jpeg', '.png', '.webp'],
  skipFiles: ['ComicInfo.xml'],
};

export class PackagerService {
  constructor(private loggerService: LoggerService) {}

  public async createCBZ(sourceDir: string, outputPath: string, comicInfo: string | null = null, retries: number = 3): Promise<boolean> {
    for (let attempt = 1; attempt <= retries; attempt++) {
      try {
        await new Promise<void>((resolve, reject) => {
          const output = createWriteStream(outputPath);
          const archive = archiver('zip', {
            zlib: { level: 9 },
          });

          output.on('close', () => resolve());
          archive.on('error', (err: Error) => reject(err));
          output.on('error', (err: Error) => reject(err));

          archive.pipe(output);

          if (comicInfo) {
            archive.append(comicInfo, { name: 'ComicInfo.xml' });
          }

          archive.directory(sourceDir, false);
          archive.finalize();
        });

        await this.loggerService.logInfo(`CBZ created: ${outputPath}`);
        return true;
      } catch (error) {
        await this.loggerService.logError(`Attempt ${attempt}/${retries} failed to create CBZ ${outputPath}`, error instanceof Error ? error : new Error(String(error)));

        if (attempt === retries) return false;
        await new Promise<void>(resolve => setTimeout(() => resolve(), 1000 * attempt));
      }
    }

    return false;
  }

  public extractCBZ(cbzPath: string, extractPath: string, options: ExtractOptions = DEFAULT_OPTIONS): Promise<any> {
    const allowedExtensions = options.allowedExtensions || DEFAULT_OPTIONS.allowedExtensions;
    const skipFiles = options.skipFiles || DEFAULT_OPTIONS.skipFiles;

    return new Promise<void>((resolve, reject) => {
      yauzl.open(cbzPath, { lazyEntries: true }, (err: Error | null, zipfile?: yauzl.ZipFile) => {
        if (err || !zipfile) return reject(err || new Error('Failed to open zip file'));

        const extractionPromises: Promise<void>[] = [];
        let hasExtractedFiles: boolean = false;

        zipfile.on('entry', (entry: yauzl.Entry) => {
          // Skip directories
          if (/\/$/.test(entry.fileName)) {
            zipfile.readEntry();
            return;
          }

          // Skip specific files
          if (skipFiles?.includes(entry.fileName)) {
            zipfile.readEntry();
            return;
          }

          // Check if file extension is allowed
          const ext = path.extname(entry.fileName).toLowerCase();
          const isAllowedExtension = allowedExtensions?.some(allowed => ext === allowed || ext === allowed.toLowerCase());

          if (!isAllowedExtension) {
            zipfile.readEntry();
            return;
          }

          const promise = new Promise<void>((resolveEntry, rejectEntry) => {
            zipfile.openReadStream(entry, (err: Error | null, readStream?: Readable) => {
              if (err || !readStream) return rejectEntry(err || new Error('Failed to open read stream'));

              try {
                const outputPath: string = path.join(extractPath, path.basename(entry.fileName));

                const writeStream = createWriteStream(outputPath);

                readStream.on('end', () => {
                  writeStream.end();
                  hasExtractedFiles = true;
                  resolveEntry();
                });

                readStream.on('error', (error: Error) => {
                  writeStream.end();
                  rejectEntry(error);
                });

                writeStream.on('error', (error: Error) => {
                  readStream.destroy();
                  rejectEntry(error);
                });

                readStream.pipe(writeStream);
              } catch (error) {
                rejectEntry(error instanceof Error ? error : new Error(String(error)));
              }
            });
          });

          extractionPromises.push(promise);
          zipfile.readEntry();
        });

        zipfile.on('end', () => {
          Promise.all(extractionPromises)
            .then(() => {
              if (!hasExtractedFiles) {
                reject(new Error('No valid images were extracted from the CBZ'));
              } else {
                resolve();
              }
            })
            .catch(reject);
        });

        zipfile.on('error', (err: Error) => reject(err));
        zipfile.readEntry();
      });
    });
  }
}
