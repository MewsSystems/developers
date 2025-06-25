export const moviesQueryKey = (search: string, page: number) =>
  ['movies', { search, page }] as const;

export const movieDetailQueryKey = (movieId: string) => ['movie', movieId] as const;
