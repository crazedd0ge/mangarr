import * as dotenv from 'dotenv';
import * as fs from 'fs/promises';
import { LoggerService } from './logger.service';
import { ProxyInterface } from '../models';



dotenv.config();

const proxyFile: string | undefined = process.env.PROXY_FILE;

export class ProxyService {
  private proxyList: ProxyInterface[];

  constructor(private loggerService: LoggerService) {
    this.proxyList = [];
  }

  async loadProxies(): Promise<ProxyInterface[]> {
    try {
      if (!proxyFile) {
        throw new Error('PROXY_FILE evnvrioment var is not set');
      }

      const data = await fs.readFile(proxyFile, 'utf8');

      this.proxyList = data
        .split('\n')
        .map(line => line.trim())
        .filter(line => line.length > 0)
        .map(line => {
          const [server, port, username, password] = line.split(':');
          return {
            server: `${server}:${port}`,
            username,
            password,
          };
        });
    } catch (err: any) {
      await this.loggerService.logError('Unable to read proxy file', err);
      throw new Error(err);
    }

    return this.proxyList;
  }

  async getRandomProxy(): Promise<ProxyInterface> {
    if (this.proxyList.length == 0) {
      await this.loadProxies();
    }

    return this.proxyList[Math.floor(Math.random() * this.proxyList.length)];
  }
}
