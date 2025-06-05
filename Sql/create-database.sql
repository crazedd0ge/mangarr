-- Active: 1749117427997@@158.220.85.48@5433@manga_db
-- Drop tables if they exist (optional, for a clean slate during development)
-- Consider the order for dropping due to foreign keys as well, or use CASCADE
/*
DROP INDEX IF EXISTS manga_chapter_idx;
DROP INDEX IF EXISTS manga_volume_idx;
DROP TABLE IF EXISTS logs;
DROP TABLE IF EXISTS job_mapping;
DROP TABLE IF EXISTS job_queue;
DROP TABLE IF EXISTS chapters;
DROP TABLE IF EXISTS volumes;
DROP TABLE IF EXISTS manga;
*/

-- Table: manga
-- Drizzle schema: export const manga = pgTable('manga', { ... });
CREATE TABLE manga (
    id SERIAL PRIMARY KEY,
    title VARCHAR(255) NOT NULL,
    url VARCHAR(255) NOT NULL UNIQUE,
    description TEXT,
    cover_image VARCHAR(255),
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

-- Table: volumes
-- Drizzle schema: export const volumes = pgTable('volumes', { ... });
CREATE TABLE volumes (
    id SERIAL PRIMARY KEY,
    manga_id INTEGER NOT NULL,
    volume_number REAL NOT NULL,
    title VARCHAR(255) NOT NULL,
    url VARCHAR(255) NOT NULL UNIQUE,
    description TEXT,
    cover_image VARCHAR(255),
    complete BOOLEAN DEFAULT false,
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    CONSTRAINT fk_manga
        FOREIGN KEY(manga_id)
        REFERENCES manga(id)
        ON DELETE CASCADE
);

-- Unique index for volumes table
-- Drizzle schema: uniqueIndex('manga_volume_idx').on(table.mangaId, table.volumeNumber)
CREATE UNIQUE INDEX manga_volume_idx ON volumes (manga_id, volume_number);

-- Table: chapters
-- Drizzle schema: export const chapters = pgTable('chapters', { ... });
CREATE TABLE chapters (
    id SERIAL PRIMARY KEY,
    manga_id INTEGER NOT NULL,
    volume_id INTEGER,
    "chapterKeyword" VARCHAR(255) NOT NULL, -- Quoted to preserve camelCase
    chapter_number REAL NOT NULL,
    title VARCHAR(255),
    url VARCHAR(255) NOT NULL UNIQUE,
    downloaded BOOLEAN DEFAULT false,
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    CONSTRAINT fk_manga
        FOREIGN KEY(manga_id)
        REFERENCES manga(id)
        ON DELETE CASCADE,
    CONSTRAINT fk_volume
        FOREIGN KEY(volume_id)
        REFERENCES volumes(id)
        ON DELETE CASCADE
);

-- Unique index for chapters table
-- Drizzle schema: uniqueIndex('manga_chapter_idx').on(table.mangaId, table.chapterNumber)
CREATE UNIQUE INDEX manga_chapter_idx ON chapters (manga_id, chapter_number);

-- Table: job_queue
-- Drizzle schema: export const jobQueue = pgTable('job_queue', { ... });
CREATE TABLE job_queue (
    id SERIAL PRIMARY KEY,
    manga_id INTEGER,
    completed BOOLEAN NOT NULL DEFAULT false,
    type VARCHAR(50) NOT NULL,
    attempts INTEGER DEFAULT 0 NOT NULL,
    max_attempts INTEGER DEFAULT 3 NOT NULL,
    last_attempt_at TIMESTAMP,
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    scheduled_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    completed_at TIMESTAMP,
    error TEXT,
    CONSTRAINT fk_manga
        FOREIGN KEY(manga_id)
        REFERENCES manga(id)
        ON DELETE SET NULL -- Or ON DELETE CASCADE, depending on desired behavior if manga is deleted
);

-- Table: job_mapping
-- Drizzle schema: export const jobMapping = pgTable('job_mapping', { ... });
CREATE TABLE job_mapping (
    id SERIAL PRIMARY KEY,
    job_id INTEGER NOT NULL,
    volume_number REAL,
    chapter_keyword VARCHAR(100) NOT NULL,
    "chapterNumber" INTEGER NOT NULL, -- Quoted to preserve camelCase
    completed BOOLEAN NOT NULL DEFAULT false,
    completed_at TIMESTAMP,
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    CONSTRAINT fk_job_queue
        FOREIGN KEY(job_id)
        REFERENCES job_queue(id)
        ON DELETE CASCADE
);

-- Table: logs
-- Drizzle schema: export const logs = pgTable('logs', { ... });
CREATE TABLE logs (
    id SERIAL PRIMARY KEY,
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    is_error BOOLEAN DEFAULT false,
    is_warning BOOLEAN DEFAULT false,
    is_info BOOLEAN DEFAULT false,
    message VARCHAR(2048)
);

-- End of script
