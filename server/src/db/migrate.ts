import { drizzle } from 'drizzle-orm/better-sqlite3';
import { migrate } from 'drizzle-orm/better-sqlite3/migrator';
import Database from 'better-sqlite3';

async function main() {
  const sqlite = new Database('data/manga.db');
  const db = drizzle(sqlite);

  console.log('Running migrations...');
  
  await migrate(db, { migrationsFolder: './src/db/migrations' });
  
  console.log('Migrations completed!');
  
  process.exit(0);
}

main().catch((err) => {
  console.error('Migration failed!');
  console.error(err);
  process.exit(1);
});