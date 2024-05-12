export interface TMDBSearchResponse<T> {
  page: number;
  results: T[];
  total_pages: number;
  total_results: number;
}

export interface BaseMovie {
  id: number;
  original_language: string;
  original_title: string;
  overview: string;
  popularity: number;
  poster_path: string | null;
  release_date: string;
  title: string;
}

export interface BaseMovieDetails extends BaseMovie {
  runtime: number;
  status: string;
  tagline: string;
}
