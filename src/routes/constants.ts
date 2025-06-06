export const PageType = {
  MoviesListPage: 'MoviesListPage',
  MovieDetailsPage: 'MovieDetailsPage',
} as const;

export const PathByPageType = {
  [PageType.MoviesListPage]: '/',
  [PageType.MovieDetailsPage]: '/movie/:id',
} as const;
