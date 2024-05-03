import { TMDB_API_KEY } from "../config";

const TMDB_API_URL = "https://api.themoviedb.org/3/";
const TMDB_IMAGES_BASE_PATH = "http://image.tmdb.org/t/p/";

export class HttpError extends Error {
  constructor(public status: number, message: string) {
    super(message);
    this.name = "HttpError";
  }
}

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

export type MovieDetail = {
  adult: boolean;
  backdrop_path?: string;
  belongs_to_collection?: boolean;
  budget: number;
  genres: [
    {
      id: number;
      name: string;
    }
  ];
  homepage?: string;
  id: number;
  imdb_id?: string;
  origin_country: string[];
  original_language: string;
  original_title: string;
  overview: string;
  popularity: number;
  poster_path?: string;
  production_companies: [
    {
      id: number;
      logo_path?: string;
      name: string;
      origin_country: string;
    }
  ];
  production_countries: [
    {
      iso_3166_1: string;
      name: string;
    }
  ];
  release_date: string;
  revenue: number;
  runtime: number;
  spoken_languages: [
    {
      english_name: string;
      iso_639_1: string;
      name: string;
    }
  ];
  status: string;
  tagline?: string;
  title: string;
  video: boolean;
  vote_average: number;
  vote_count: number;
};

export type PosterSize =
  | "w92"
  | "w154"
  | "w185"
  | "w342"
  | "w500"
  | "w780"
  | "original";

export type BackdropSize = "w300" | "w780" | "w1280" | "original";

async function get(path: string, init?: RequestInit) {
  const paramSeparator = path.includes("?") ? "&" : "?";
  const response = await fetch(
    `${TMDB_API_URL}/${path}${paramSeparator}api_key=${TMDB_API_KEY}`,
    {
      method: "GET",
      headers: {
        accept: "application/json",
      },
      ...init,
    }
  );

  if (!response.ok) {
    throw new HttpError(response.status, "Network response was not ok");
  }

  return response.json();
}

export function findMovies(
  query: string = "",
  page: number = 1,
  init?: RequestInit
): Promise<Results<Movie>> {
  return get(
    `search/movie?query=${encodeURIComponent(
      query
    )}&include_adult=true&language=en-US&page=${page}`,
    init
  );
}

export function fetchMovieById(id: string): Promise<MovieDetail> {
  return get(`movie/${id}`);
}

export function posterUrl(path: string, size: PosterSize) {
  return `${TMDB_IMAGES_BASE_PATH}${size}${path}`;
}

export function backdropUrl(path: string, size: BackdropSize) {
  return `${TMDB_IMAGES_BASE_PATH}${size}${path}`;
}
