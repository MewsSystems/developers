import { createUsePaginatedMoviesHook } from "~/hooks/use-paginated-movie-factory";
import { api } from "~/trpc/react";
import type { z } from "zod";
import type { getPopularMovesInputSchema } from "~/schemas/movies.schemas";

export const usePopularMovies = createUsePaginatedMoviesHook<
  z.infer<typeof getPopularMovesInputSchema>
>((input, options) => api.movie.getPopular.useQuery(input, options));
