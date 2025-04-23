import { LogWorker } from '../workers';

export class LoggerService {
  constructor(private logWorker: LogWorker) {}

  async logError(message: string, error: Error) {
    const logMessage: string = `${message}\n ${error.stack || error}\n\n`;

    await this.logWorker.writeError(logMessage);
  }

  async logWarning(message: string) {
    await this.logWorker.writeWarning(message);
  }

  async logInfo(message: string) {
    await this.logWorker.writeInfo(message);
  }
}
