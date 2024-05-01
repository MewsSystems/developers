import { FMDB_API_KEY } from "../config";
import delay from "./delay";

const FMDB_API_URL = "https://api.themoviedb.org/3";

export type Results<T> = {
  page: number;
  results: T[];
  total_pages: number;
  total_results: number;
};

export type Movie = {
  adult: boolean;
  backdrop_path?: string;
  genre_ids: number[];
  id: number;
  original_language: string;
  original_title: string;
  overview?: string;
  popularity: number;
  poster_path?: string;
  release_date: string;
  title: string;
  video: boolean;
  vote_average: number;
  vote_count: number;
};

async function get(path: string, init?: RequestInit) {
  const response = await fetch(
    `${FMDB_API_URL}/${path}&api_key=${FMDB_API_KEY}`,
    {
      method: "GET",
      headers: {
        accept: "application/json",
      },
      ...init,
    }
  );

  if (!response.ok) {
    throw new Error("Network response was not ok");
  }

  return response.json();
}

export function findMovies(
  query: string = "",
  page: number = 1,
  init?: RequestInit
): Promise<Results<Movie>> {
  return get(
    `/search/movie?query=${encodeURIComponent(query)}&include_adult=true&language=en-US&page=${page}`,
    init
  );
}
