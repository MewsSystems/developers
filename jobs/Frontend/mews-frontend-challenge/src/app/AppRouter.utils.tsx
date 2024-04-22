import queryString from "query-string";

export const SEARCH_ROUTE = "/";
export const MOVIE_DETAIL_ROUTE = "/movie/:id";

export function getSearchRoute({
  q,
  page,
}: { q?: string; page?: number } = {}) {
  return queryString.stringifyUrl({ url: SEARCH_ROUTE, query: { q, page } });
}

export function getMovieDetailRoute(id: number) {
  return MOVIE_DETAIL_ROUTE.replace(":id", id.toString());
}
