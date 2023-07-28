export interface MovieItemResponse {
  adult: boolean;
  id: number;
  original_language: string;
  original_title: string;
  overview: string;
  popularity: number;
  poster_path: string;
  release_date: string;
  title: string;
  vote_average: number;
  vote_count: number;
}

export interface MoviesPageResponse {
  page: number;
  results: MovieItemResponse[];
  total_pages: number;
  total_results: number;
}

export interface MovieDetailResponse {
  adult: boolean;
  budget: number;
  genres: string;
  id: number;
  original_language: string;
  original_title: string;
  overview: string;
  popularity: number;
  poster_path: string;
  release_date: string;
  revenue: number;
  runtime: number;
  status: string;
  tagline: string;
  title: string;
  vote_average: number;
  vote_count: number;
}
