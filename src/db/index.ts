import "dotenv/config";
import { drizzle } from "drizzle-orm/node-postgres";
import * as schema from "./schema.js";
import { Pool } from "pg";

const pool = new Pool({
  connectionString: process.env.DATABASE_URL,
});

export const db = drizzle(pool, { schema });

export async function testConnection() {
  try {
    const result = await db.select().from(schema.chapters).limit(1);
    console.log("Connected to PostgreSQL successfully");
    return true;
  } catch (error) {
    console.error("Database connection error:", error);
    return false;
  }
}

export async function closePool() {
  await pool.end();
}


export * from "../db/schema.js"