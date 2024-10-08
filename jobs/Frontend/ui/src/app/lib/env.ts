import { z } from "zod"

const envSchema = z.object({
  MOVIE_DB_API_ACCESS_TOKEN: z.string(),
  MOVIE_DB_API_BASE_URL: z.string().url(),
})

export const env = envSchema.parse(process.env)

export type Env = z.infer<typeof envSchema>
