export const paths = {
  home: () => "/",
  discovery: () => "/discovery",
  movies: () => "/movies",
  movie: (id: number) => `/movies/${id}`,
  tvShows: () => "/tv-shows",
  tvShow: (id: number) => `/tv-shows/${id}`,
};
