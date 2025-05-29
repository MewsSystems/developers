export const PageType = {
  MoviesListPage: 'MoviesListPage',
  MovieDetailsPage: 'MovieDetailsPage',
} as const;

export const PathByPageType = {
  [PageType.MoviesListPage]: '/',
  [PageType.MovieDetailsPage]: '/movies/:id',
} as const;
