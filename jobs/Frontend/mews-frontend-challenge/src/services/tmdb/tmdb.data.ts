import { MovieSearchOptions } from "tmdb-ts/dist/endpoints";
import {
  AppendToResponse,
  AppendToResponseMovieKey,
  Movie,
  Search,
} from "tmdb-ts/dist/types";
import { MovieDetails } from "./tmdb.types";

const BASE_TMDB_URL_V3 = "https://api.themoviedb.org/3";
const TMDB_API_KEY = import.meta.env.VITE_THEMOVIEDB_API_KEY;

async function getTmdbRequest<Response, Options extends object>(
  path: string,
  options?: Options,
): Promise<Response> {
  const params = parseOptions({ api_key: TMDB_API_KEY, ...options });
  const response = await fetch(`${BASE_TMDB_URL_V3}${path}?${params}`, {
    method: "GET",
    headers: {
      "Content-Type": "application/json;charset=utf-8",
    },
  });

  if (!response.ok) {
    return Promise.reject(await response.json());
  }

  return (await response.json()) as Response;
}

export function parseOptions(options?: object): string {
  return options ? new URLSearchParams(Object.entries(options)).toString() : "";
}

// tmdb-ts is a great library but only allows auth via accessToken, not apiKey
// Due to this, we reuse the typings and re-implement their api methods since we only use a few of them
// TODO: Request a PR to use apiKey in tmdb-ts
export const tmdb = {
  search: {
    movies: (options: MovieSearchOptions) =>
      getTmdbRequest<Promise<Search<Movie>>, MovieSearchOptions>(
        "/search/movie",
        options,
      ),
  },
  movies: {
    details: <T extends AppendToResponseMovieKey[] | undefined>(
      id: number,
      appendToResponse?: T,
      language?: string,
    ) => {
      const options = {
        append_to_response: appendToResponse
          ? appendToResponse.join(",")
          : undefined,
        language: language,
      };

      return getTmdbRequest<
        AppendToResponse<MovieDetails, T, "movie">,
        typeof options
      >(`/movie/${id}`, options);
    },
  },
};
