import { SerializedError } from "@reduxjs/toolkit";

export interface MoviesListState {
  searchKey: string;
  movies: MoviesSearchResult[];
  isBusy: boolean;
  error: SerializedError | null;
  activePage: number;
  totalPages: number;
  results: number;
  movieDetail: MovieDetailResult | null;
  movieDetailId: string;
}

export interface MoviesSearchResponse {
  page: number;
  results: MoviesSearchResult[];
  total_pages: number;
  total_results: number;
}

export interface MoviesSearchResult {
  adult: boolean;
  backdrop_path: string | null;
  genre_ids?: MovieCategoryCollection[];
  id: number;
  original_language: string;
  original_title: string;
  overview: string;
  popularity: number;
  poster_path: string | null;
  release_date: string;
  title: string;
  video: boolean;
  vote_average: number;
  vote_count: number;
}

export interface MovieCategoryCollection {
  id: number;
  name: string;
}

export interface MovieDetailResult extends MoviesSearchResult {
  belongs_to_collection: null | string;
  budget: number;
  genres: MovieCategoryCollection[];
  homepage: string;
  production_companies: MovieCategoryCollection[];
  production_countries: MovieCategoryCollection[];
  revenue: number;
  runtime: number;
  spoken_languages: MovieCategoryCollection[];
  status: string;
  tagline: string;
  title: string;
  video: boolean;
  vote_average: number;
  vote_count: number;
}
