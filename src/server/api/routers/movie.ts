import { z } from "zod";
import { createTRPCRouter, publicProcedure } from "../trpc";
import type { MovieApiResponse, MovieDetailExtended } from "~/types/movie";
import {
  creditsSchema,
  getPopularMovesInputSchema,
  movieDetailsSchema,
  searchMoviesInputSchema,
  tmdbResponseSchema,
} from "~/schemas/movies.schemas";

/* TMDB API has a limit of 500 pages for paginated requests
  This function limits the total pages to 500
 */
const limitTotalPages = (input: MovieApiResponse): MovieApiResponse => {
  return { ...input, total_pages: Math.min(input.total_pages, 500) };
};

export const movieRouter = createTRPCRouter({
  getPopular: publicProcedure
    .input(getPopularMovesInputSchema)
    .query(async ({ input: { page = 1 }, ctx }) => {
      const res = await ctx.client.get("/movie/popular", {
        params: { page },
      });
      const data = tmdbResponseSchema.safeParse(res.data);
      if (data.error) {
        throw new Error("Data is not valid");
      }
      return limitTotalPages(data.data);
    }),

  search: publicProcedure
    .input(searchMoviesInputSchema)
    .query(async ({ input: { query, page = 1 }, ctx }) => {
      const res = await ctx.client.get("/search/movie", {
        params: { query, page },
      });
      const data = tmdbResponseSchema.safeParse(res.data);
      if (data.error) {
        throw new Error("Data is not valid");
      }
      return limitTotalPages(data.data);
    }),
  getDetails: publicProcedure
    .input(z.object({ id: z.number() }))
    .query(async ({ input: { id }, ctx }): Promise<MovieDetailExtended> => {
      const [detailRes, creditsRes] = await Promise.all([
        ctx.client.get(`/movie/${id}`),
        ctx.client.get(`/movie/${id}/credits`),
      ]);

      const details = movieDetailsSchema.parse(detailRes.data);
      const credits = creditsSchema.parse(creditsRes.data);

      const director = credits.crew.find((member) => member.job === "Director");
      const cast = credits.cast.slice(0, 10);

      return {
        ...details,
        director: director ? { name: director.name, id: director.id } : null,
        cast,
      };
    }),
});
