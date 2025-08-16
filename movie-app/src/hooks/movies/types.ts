interface Genre {
  id: number;
  name: string;
}

export interface Movie {
  id: number;
  original_language: string;
  original_title: string;
  overview: string;
  poster_path?: string | null;
  release_date: string;
  title: string;
  vote_average?: number;
  vote_count?: number;
  tagline?: string;
  genres: Genre[];
}

export interface MoviesSearchApiResponse {
  page: number;
  results: Movie[];
  total_pages: number;
  total_results: number;
}
