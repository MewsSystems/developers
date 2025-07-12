export const moviesQueryKey = (search: string, page: number) =>
  ['movies', { search, page }] as const;
