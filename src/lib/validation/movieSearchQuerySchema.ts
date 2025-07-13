import { z } from 'zod';

export const movieSearchQuerySchema = z.object({
  search: z.string().trim().min(1).max(100),
  page: z.coerce.number().int().positive().max(1000).default(1),
});
