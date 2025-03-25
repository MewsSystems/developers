import { createUsePaginatedMoviesHook } from "~/hooks/use-paginated-movie-factory";
import { api } from "~/trpc/react";
import type { z } from "zod";
import type { searchMoviesInputSchema } from "~/schemas/movies.schemas";

export const useSearchMovies = createUsePaginatedMoviesHook<
  z.infer<typeof searchMoviesInputSchema>
>((input, options) => api.movie.search.useQuery(input, options));
