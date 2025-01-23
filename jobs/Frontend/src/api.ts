import { z } from "zod";

const API_BASE_URL = "https://api.themoviedb.org/3";
const API_KEY = "03b8572954325680265531140190fd2a";

export const movieSchema = z.object({
  title: z.string(),
  overview: z.string(),
  popularity: z.number(),
  id: z.number(),
  release_date: z.string().transform((date) => date.substring(0, 4)),
});

export const movieDbResponseSchema = z.object({
  page: z.number(),
  results: z.array(movieSchema),
  total_pages: z.number(),
  total_results: z.number(),
});

export type Movie = z.infer<typeof movieSchema>;
export type MovieDbResponse = z.infer<typeof movieDbResponseSchema>;

export async function getMovies(
  searchString: string,
  context: { pageParam: number }
): Promise<MovieDbResponse> {
  const url = new URL(`${API_BASE_URL}/search/movie`);
  const params = new URLSearchParams({
    query: encodeURIComponent(searchString),
    api_key: API_KEY,
    page: context.pageParam.toString(),
  });

  url.search = params.toString();

  const res = await fetch(url);
  return movieDbResponseSchema.parse(await res.json());
}
