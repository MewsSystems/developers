import { fetchJson } from "@/lib/fetchJson";
import type { SearchResponse, TmdbMovieDetail, CreditsResponse } from "@/types";

const base = process.env.NEXT_PUBLIC_BASE_PATH ?? "";

export async function searchMovies(query: string, pageNumber = 1): Promise<SearchResponse> {
  const url = `${base}/api/search?q=${encodeURIComponent(query)}&page=${pageNumber}`;
  return fetchJson<SearchResponse>(url, { cache: "no-store" });
}

export async function getMovieById(movieId: string): Promise<TmdbMovieDetail> {
  const url = `${base}/api/movie/${movieId}`;
  return fetchJson<TmdbMovieDetail>(url, { cache: "no-store" });
}

export async function getMovieCredits(movieId: string): Promise<CreditsResponse> {
  const url = `${base}/api/movie/${movieId}/credits`;
  return fetchJson<CreditsResponse>(url, { cache: "no-store" });
}