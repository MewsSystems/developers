import { MovieResult, SearchParams, Movie } from "./types";

const KEY = '03b8572954325680265531140190fd2a';
const MOVIES_URL = 'https://api.themoviedb.org/3/'

const movieFetch = (path: string, params: any = {}) => {
  const url = new URL(`${MOVIES_URL}${path}?api_key=${KEY}`);

  Object.keys(params).forEach((k) => {
    url.searchParams.append(k, params[k]);
  })
  return fetch(url.toString()).then(q => q.json());
}

export type SearchResult = {
  page: number,
  results: MovieResult[],
  total_results: number,
  total_pages: number
}


export function search(params: SearchParams): Promise<SearchResult> {
  return movieFetch('search/movie', params);
}

export function getMovie(id: number): Promise<Movie> {
  return movieFetch(`movie/${id}`);
}
