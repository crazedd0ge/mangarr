import { bindIfParam, InferInsertModel, InferSelectModel, relations } from 'drizzle-orm';
import { boolean, integer, pgTable, real, serial, text, timestamp, uniqueIndex, varchar } from 'drizzle-orm/pg-core';

export const manga = pgTable('manga', {
  id: serial('id').primaryKey(),
  title: varchar('title', { length: 255 }).notNull(),
  url: varchar('url', { length: 255 }).notNull().unique(),
  description: text('description'),
  coverImage: varchar('cover_image', { length: 255 }),
  createdAt: timestamp('created_at').defaultNow(),
  updatedAt: timestamp('updated_at').defaultNow(),
});

export const volumes = pgTable(
  'volumes',
  {
    id: serial('id').primaryKey(),
    mangaId: integer('manga_id')
      .references(() => manga.id, { onDelete: 'cascade' })
      .notNull(),
    volumeNumber: real('volume_number').notNull(),
    title: varchar('title', { length: 255 }).notNull(),
    url: varchar('url', { length: 255 }).notNull().unique(),
    description: text('description'),
    coverImage: varchar('cover_image', { length: 255 }),
    complete: boolean('complete').default(false),
    createdAt: timestamp('created_at').defaultNow(),
    updatedAt: timestamp('updated_at').defaultNow(),
  },
  table => {
    return {
      mangaChapterIdx: uniqueIndex('manga_volume_idx').on(table.mangaId, table.volumeNumber),
    };
  }
);

export const chapters = pgTable(
  'chapters',
  {
    id: serial('id').primaryKey(),
    mangaId: integer('manga_id')
      .references(() => manga.id, { onDelete: 'cascade' })
      .notNull(),
    volumeId: integer('volume_id').references(() => volumes.id, {
      onDelete: 'cascade',
    }),
    chapterKeyword: varchar('chapterKeyword', { length: 255 }).notNull(),
    chapterNumber: real('chapter_number').notNull(),
    title: varchar('title', { length: 255 }),
    url: varchar('url', { length: 255 }).notNull().unique(),
    downloaded: boolean('downloaded').default(false),
    createdAt: timestamp('created_at').defaultNow(),
    updatedAt: timestamp('updated_at').defaultNow(),
  },
  table => {
    return {
      mangaChapterIdx: uniqueIndex('manga_chapter_idx').on(table.mangaId, table.chapterNumber),
    };
  }
);

export const jobQueue = pgTable('job_queue', {
  id: serial('id').primaryKey(),
  mangaId: integer('manga_id').references(() => manga.id),
  completed: boolean('completed').notNull().default(false),
  type: varchar('type', { length: 50 }).notNull(), // "download_manga", "download_volume", etc.
  attempts: integer('attempts').default(0).notNull(),
  maxAttempts: integer('max_attempts').default(3).notNull(),
  lastAttemptAt: timestamp('last_attempt_at'),
  createdAt: timestamp('created_at').defaultNow(),
  updatedAt: timestamp('updated_at').defaultNow(),
  scheduledAt: timestamp('scheduled_at').defaultNow(),
  completedAt: timestamp('completed_at'),
  error: text('error'),
});

export const jobMapping = pgTable('job_mapping', {
  id: serial('id').primaryKey(),
  jobId: integer('job_id')
    .references(() => jobQueue.id, { onDelete: 'cascade' })
    .notNull(),
  volumeNumber: real('volume_number'),
  chapterKeyword: varchar('chapter_keyword', { length: 100 }).notNull(),
  chapterNumber: integer('chapterNumber').notNull(),
  completed: boolean('completed').notNull().default(false),
  completedAt: timestamp('completed_at'),
  createdAt: timestamp('created_at').defaultNow(),
  updatedAt: timestamp('updated_at').defaultNow(),
});

export const logs = pgTable('logs', {
  id: serial('id').primaryKey(),
  createdAt: timestamp('created_at').defaultNow(),
  isError: boolean('is_error').default(false),
  isWarning: boolean('is_warning').default(false),
  isInfo: boolean('is_info').default(false),
  message: varchar('message', { length: 2048 }),
});

export const mangaRelations = relations(manga, ({ many }) => ({
  volumes: many(volumes),
  chapters: many(chapters),
}));

export const volumeRelations = relations(volumes, ({ one }) => ({
  manga: one(manga, {
    fields: [volumes.mangaId],
    references: [manga.id],
  }),
}));

export const chapterVolumeRelations = relations(chapters, ({ one }) => ({
  volumes: one(volumes, {
    fields: [chapters.volumeId],
    references: [volumes.id],
  }),
}));

export const chapterRelations = relations(chapters, ({ one }) => ({
  manga: one(manga, {
    fields: [chapters.mangaId],
    references: [manga.id],
  }),
}));

export const jobRelations = relations(jobQueue, ({ one, many }) => ({
  manga: one(manga, {
    fields: [jobQueue.mangaId],
    references: [manga.id],
  }),
  mappings: many(jobMapping),
}));

export const jobMappingRelations = relations(jobMapping, ({ one }) => ({
  job: one(jobQueue, {
    fields: [jobMapping.jobId],
    references: [jobQueue.id],
  }),
}));

export type Manga = InferSelectModel<typeof manga>;
export type Volume = InferSelectModel<typeof volumes>;
export type Chapter = InferSelectModel<typeof chapters>;
export type Log = InferSelectModel<typeof logs>;
export type JobQueue = InferSelectModel<typeof jobQueue>;
export type JobMapping = InferSelectModel<typeof jobMapping>;

// Insert types (improved with optional auto-generated and default fields)
export type NewManga = Omit<InferInsertModel<typeof manga>, 'id' | 'createdAt' | 'updatedAt'> & {
  id?: number;
  createdAt?: Date;
  updatedAt?: Date;
};

export type NewVolume = Omit<InferInsertModel<typeof volumes>, 'id' | 'complete' | 'createdAt' | 'updatedAt'> & {
  id?: number;
  complete?: boolean;
  createdAt?: Date;
  updatedAt?: Date;
};

export type NewChapter = Omit<InferInsertModel<typeof chapters>, 'id' | 'downloaded' | 'createdAt' | 'updatedAt'> & {
  id?: number;
  downloaded?: boolean;
  createdAt?: Date;
  updatedAt?: Date;
};

export type NewLog = Omit<InferInsertModel<typeof logs>, 'id' | 'createdAt' | 'isError' | 'isWarning' | 'isInfo'> & {
  id?: number;
  createdAt?: Date;
  isError?: boolean;
  isWarning?: boolean;
  isInfo?: boolean;
};

export type NewJobQueue = Omit<InferInsertModel<typeof jobQueue>, 'id' | 'completed' | 'attempts' | 'maxAttempts' | 'createdAt' | 'updatedAt' | 'scheduledAt' | 'completedAt'> & {
  id?: number;
  completed?: boolean;
  attempts?: number;
  maxAttempts?: number;
  createdAt?: Date;
  updatedAt?: Date;
  scheduledAt?: Date;
  completedAt?: Date | null;
  error?: string | null;
};

export type NewJobMapping = Omit<InferInsertModel<typeof jobMapping>, 'id' | 'createdAt' | 'updatedAt' | 'jobId'> & {
  id?: number;
  jobId?: number;
  createdAt?: Date;
  updatedAt?: Date;
};
