
public static class SqlScripts
{
    #region Table Creation Scripts

    public const string CreateMangaTable = @"
            CREATE TABLE IF NOT EXISTS manga (
                id SERIAL PRIMARY KEY,
                title VARCHAR(255) NOT NULL,
                url VARCHAR(255) NOT NULL UNIQUE,
                description TEXT,
                cover_image VARCHAR(255),
                created_at TIMESTAMP DEFAULT NOW(),
                updated_at TIMESTAMP DEFAULT NOW()
            );";

    public const string CreateVolumesTable = @"
            CREATE TABLE IF NOT EXISTS volumes (
                id SERIAL PRIMARY KEY,
                manga_id INTEGER NOT NULL,
                volume_number REAL NOT NULL,
                title VARCHAR(255) NOT NULL,
                url VARCHAR(255) NOT NULL UNIQUE,
                description TEXT,
                cover_image VARCHAR(255),
                complete BOOLEAN DEFAULT FALSE,
                created_at TIMESTAMP DEFAULT NOW(),
                updated_at TIMESTAMP DEFAULT NOW(),
                FOREIGN KEY (manga_id) REFERENCES manga(id) ON DELETE CASCADE
            );";

    public const string CreateChaptersTable = @"
            CREATE TABLE IF NOT EXISTS chapters (
                id SERIAL PRIMARY KEY,
                manga_id INTEGER NOT NULL,
                volume_id INTEGER,
                chapter_keyword VARCHAR(255) NOT NULL,
                chapter_number REAL NOT NULL,
                title VARCHAR(255),
                url VARCHAR(255) NOT NULL UNIQUE,
                downloaded BOOLEAN DEFAULT FALSE,
                created_at TIMESTAMP DEFAULT NOW(),
                updated_at TIMESTAMP DEFAULT NOW(),
                FOREIGN KEY (manga_id) REFERENCES manga(id) ON DELETE CASCADE,
                FOREIGN KEY (volume_id) REFERENCES volumes(id) ON DELETE CASCADE
            );";

    public const string CreateJobQueueTable = @"
            CREATE TABLE IF NOT EXISTS job_queue (
                id SERIAL PRIMARY KEY,
                manga_id INTEGER,
                completed BOOLEAN NOT NULL DEFAULT FALSE,
                type VARCHAR(50) NOT NULL,
                attempts INTEGER DEFAULT 0 NOT NULL,
                max_attempts INTEGER DEFAULT 3 NOT NULL,
                last_attempt_at TIMESTAMP,
                created_at TIMESTAMP DEFAULT NOW(),
                updated_at TIMESTAMP DEFAULT NOW(),
                scheduled_at TIMESTAMP DEFAULT NOW(),
                completed_at TIMESTAMP,
                error TEXT,
                FOREIGN KEY (manga_id) REFERENCES manga(id)
            );";

    public const string CreateJobMappingTable = @"
            CREATE TABLE IF NOT EXISTS job_mapping (
                id SERIAL PRIMARY KEY,
                job_id INTEGER NOT NULL,
                volume_number REAL,
                chapter_keyword VARCHAR(100) NOT NULL,
                chapter_number INTEGER NOT NULL,
                completed BOOLEAN NOT NULL DEFAULT FALSE,
                completed_at TIMESTAMP,
                created_at TIMESTAMP DEFAULT NOW(),
                updated_at TIMESTAMP DEFAULT NOW(),
                FOREIGN KEY (job_id) REFERENCES job_queue(id) ON DELETE CASCADE
            );";

    public const string CreateLogsTable = @"
            CREATE TABLE IF NOT EXISTS logs (
                id SERIAL PRIMARY KEY,
                created_at TIMESTAMP DEFAULT NOW(),
                is_error BOOLEAN DEFAULT FALSE,
                is_warning BOOLEAN DEFAULT FALSE,
                is_info BOOLEAN DEFAULT FALSE,
                message VARCHAR(2048)
            );";

    #endregion

    #region Indexes (PostgreSQL specific)

    public const string CreateIndexes = @"
            CREATE UNIQUE INDEX IF NOT EXISTS manga_volume_idx ON volumes(manga_id, volume_number);
            CREATE UNIQUE INDEX IF NOT EXISTS manga_chapter_idx ON chapters(manga_id, chapter_number);
            CREATE INDEX IF NOT EXISTS idx_manga_url ON manga(url);
            CREATE INDEX IF NOT EXISTS idx_chapters_manga_id ON chapters(manga_id);
            CREATE INDEX IF NOT EXISTS idx_chapters_volume_id ON chapters(volume_id);
            CREATE INDEX IF NOT EXISTS idx_volumes_manga_id ON volumes(manga_id);
            CREATE INDEX IF NOT EXISTS idx_job_queue_manga_id ON job_queue(manga_id);
            CREATE INDEX IF NOT EXISTS idx_job_queue_type ON job_queue(type);
            CREATE INDEX IF NOT EXISTS idx_job_queue_completed ON job_queue(completed);
            CREATE INDEX IF NOT EXISTS idx_job_mapping_job_id ON job_mapping(job_id);";

    #endregion

    #region Manga Queries

    public static class Manga
    {
        public const string GetAll = @"
                SELECT id, title, url, description, cover_image, created_at, updated_at
                FROM manga
                ORDER BY created_at DESC";

        public const string GetById = @"
                SELECT id, title, url, description, cover_image, created_at, updated_at
                FROM manga
                WHERE id = @Id";

        public const string GetByName = @"
                SELECT id, title, url, description, cover_image, created_at, updated_at
                FROM manga
                WHERE title = LIKE ''%|| @Name || '%'";

        public const string Insert = @"
                INSERT INTO manga (title, url, description, cover_image, created_at, updated_at)
                VALUES (@Title, @Url, @Description, @CoverImage, @CreatedAt, @UpdatedAt)
                RETURNING id;";

        public const string Update = @"
                UPDATE manga
                SET title = @Title, url = @Url, description = @Description,
                    cover_image = @CoverImage, updated_at = @UpdatedAt
                WHERE id = @Id";

        public const string Delete = "DELETE FROM manga WHERE id = @Id RETURNING id;";

    }

    #endregion

    #region Volume Queries

    public static class Volumes
    {
        public const string GetAll = @"
                SELECT id, manga_id, volume_number, title, url,
                       description, cover_image, complete,
                       created_at, updated_at
                FROM volumes
                ORDER BY manga_id, volume_number";

        public const string GetById = @"
                SELECT id, manga_id, volume_number, title, url,
                       description, cover_image, complete,
                       created_at, updated_at
                FROM volumes
                WHERE id = @Id";

        public const string GetByMangaId = @"
                SELECT id, manga_id, volume_number, title, url,
                       description, cover_image, complete,
                       created_at, updated_at
                FROM volumes
                WHERE manga_id = @MangaId
                ORDER BY volume_number";

        public const string Insert = @"
                INSERT INTO volumes (manga_id, volume_number, title, url, description, cover_image, complete, created_at, updated_at)
                VALUES (@MangaId, @VolumeNumber, @Title, @Url, @Description, @CoverImage, @Complete, @CreatedAt, @UpdatedAt)
                RETURNING id;";

        public const string Update = @"
                UPDATE volumes
                SET manga_id = @MangaId, volume_number = @VolumeNumber, title = @Title,
                    url = @Url, description = @Description, cover_image = @CoverImage,
                    complete = @Complete, updated_at = @UpdatedAt
                WHERE id = @Id";

        public const string Delete = "DELETE FROM volumes WHERE id = @Id";
    }

    #endregion

    #region Chapter Queries

    public static class Chapters
    {
        public const string GetAll = @"
                SELECT id, manga_id, volume_id, chapter_keyword,
                       chapter_number, title, url, downloaded,
                       created_at, updated_at
                FROM chapters
                ORDER BY manga_id, chapter_number";

        public const string GetById = @"
                SELECT id, manga_id, volume_id, chapter_keyword,
                       chapter_number, title, url, downloaded,
                       created_at, updated_at
                FROM chapters
                WHERE id = @Id";

        public const string GetByMangaId = @"
                SELECT id, manga_id, volume_id, chapter_keyword,
                       chapter_number, title, url, downloaded,
                       created_at, updated_at
                FROM chapters
                WHERE manga_id = @MangaId
                ORDER BY chapter_number";

        public const string GetByVolumeId = @"
                SELECT id, manga_id, volume_id, chapter_keyword,
                       chapter_number, title, url, downloaded,
                       created_at, updated_at
                FROM chapters
                WHERE volume_id = @VolumeId
                ORDER BY chapter_number";

        public const string Insert = @"
                INSERT INTO chapters (manga_id, volume_id, chapter_keyword, chapter_number, title, url, downloaded, created_at, updated_at)
                VALUES (@MangaId, @VolumeId, @ChapterKeyword, @ChapterNumber, @Title, @Url, @Downloaded, @CreatedAt, @UpdatedAt)
                RETURNING id;";

        public const string Update = @"
                UPDATE chapters
                SET manga_id = @MangaId, volume_id = @VolumeId, chapter_keyword = @ChapterKeyword,
                    chapter_number = @ChapterNumber, title = @Title, url = @Url,
                    downloaded = @Downloaded, updated_at = @UpdatedAt
                WHERE id = @Id";

        public const string MarkAsDownloaded = @"
                UPDATE chapters
                SET downloaded = TRUE, updated_at = @UpdatedAt
                WHERE id = @Id";

        public const string Delete = "DELETE FROM chapters WHERE id = @Id";
    }

    #endregion

    #region Job Queue Queries

    public static class JobQueue
    {
        public const string GetAll = @"
                SELECT id, manga_id, completed, type, attempts, max_attempts,
                       last_attempt_at, created_at, updated_at,
                       scheduled_at, completed_at, error
                FROM job_queue
                ORDER BY scheduled_at";

        public const string GetPending = @"
                SELECT id, manga_id, completed, type, attempts, max_attempts,
                       last_attempt_at, created_at, updated_at,
                       scheduled_at, completed_at, error
                FROM job_queue
                WHERE completed = FALSE AND attempts < max_attempts
                ORDER BY scheduled_at";

        public const string Insert = @"
                INSERT INTO job_queue (manga_id, completed, type, attempts, max_attempts, created_at, updated_at, scheduled_at)
                VALUES (@MangaId, @Completed, @Type, @Attempts, @MaxAttempts, @CreatedAt, @UpdatedAt, @ScheduledAt)
                RETURNING id;";

        public const string MarkCompleted = @"
                UPDATE job_queue
                SET completed = TRUE, completed_at = @CompletedAt, updated_at = @UpdatedAt
                WHERE id = @Id";

        public const string IncrementAttempts = @"
                UPDATE job_queue
                SET attempts = attempts + 1, last_attempt_at = @LastAttemptAt,
                    error = @Error, updated_at = @UpdatedAt
                WHERE id = @Id";
    }

    #endregion

    #region Logs Queries

    public static class Logs
    {
        public const string GetErrors = @"
         SELECT id, created_at, message
                FROM logs
                WHERE created_at between @StartDate and @EndDate and IsError = 1 and message LIKE '%' || @SearchTerm ||'%'
                ORDER BY created_at DESC";

        public const string GetWarnings = @"
         SELECT id, created_at, message
                FROM logs
                WHERE created_at between @StartDate and @EndDate and IsWarning = 1 and message LIKE '%' || @SearchTerm ||'%'
                ORDER BY created_at DESC";

        public const string GetAll = @"
                SELECT id, created_at, is_error, is_warning, is_info, message
                FROM logs
                ORDER BY created_at DESC";

        public const string GetRecent = @"
                SELECT id, created_at, is_error, is_warning, is_info, message
                FROM logs
                ORDER BY created_at DESC
                LIMIT @Limit";

        public const string Insert = @"
                INSERT INTO logs (created_at, is_error, is_warning, is_info, message)
                VALUES (@CreatedAt, @IsError, @IsWarning, @IsInfo, @Message)
                RETURNING id;";
    }

    #endregion
}
