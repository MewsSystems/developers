export const ROUTES = {
  HOME: "/",
  MOVIE_DETAIL: "/movie/:id",
  NOT_FOUND: "*",
} as const

export const buildMovieDetailRoute = (movieId: number | string): string => {
  return `/movie/${movieId}`
}
