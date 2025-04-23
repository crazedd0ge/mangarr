import { Log, logs, NewLog } from '../db/index';
import { db } from '../db/index';
import { and, between, eq, like } from 'drizzle-orm';

export class LogWorker {
  public async getErrors(startTime: Date, endTime: Date): Promise<Log[]> {
    return await db
      .select()
      .from(logs)
      .where(and(between(logs.createdAt, startTime, endTime), eq(logs.isError, true)));
  }

  public async getWarnings(startTime: Date, endTime: Date): Promise<Log[]> {
    return await db
      .select()
      .from(logs)
      .where(and(between(logs.createdAt, startTime, endTime), eq(logs.isWarning, true)));
  }

  public async getInfo(startTime: Date, endTime: Date): Promise<Log[]> {
    return await db
      .select()
      .from(logs)
      .where(and(between(logs.createdAt, startTime, endTime), eq(logs.isInfo, true)));
  }

  public async writeError(message: string): Promise<Log> {
    const record: Partial<Log> = {
      isError: true,
      message: message,
    };

    const response = await this.writeLog(record);

    return response;
  }

  public async writeWarning(message: string): Promise<Log> {
    const record: Partial<Log> = {
      isWarning: true,
      message: message,
    };

    const response = await this.writeLog(record);

    return response;
  }

  public async writeInfo(message: string): Promise<Log> {
    const record: Partial<Log> = {
      isInfo: true,
      message: message,
    };

    const response = await this.writeLog(record);

    return response;
  }

  private async writeLog(data: Partial<Omit<NewLog, 'id' | 'createdAt' | 'isError' | 'isWarning' | 'isInfo'>>): Promise<Log> {
    const [response] = await db.insert(logs).values(data).returning();

    return response;
  }
}
