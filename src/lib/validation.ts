import { z } from 'zod';

export const searchParamsSchema = z.object({
  search: z.string().trim().min(1).optional(),
  page: z.coerce.number().int().min(1).optional(),
});

export type SearchParams = z.infer<typeof searchParamsSchema>;
