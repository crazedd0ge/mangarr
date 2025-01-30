import { sqliteTable, text, integer, real } from 'drizzle-orm/sqlite-core';

export const config = sqliteTable('config', {
    id: integer('id').primaryKey()
})