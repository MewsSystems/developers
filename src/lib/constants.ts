export const ROUTES = {
  HOME: '/',
  MOVIE: '/movie',
  MOVIE_DETAIL: (id: number) => `${ROUTES.MOVIE}/${id}`,
} as const
