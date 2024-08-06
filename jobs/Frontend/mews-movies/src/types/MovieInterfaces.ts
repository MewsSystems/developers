export interface MoviesResponse {
  results: Movie[];
  total_pages: number;
}

export interface Movie {
  id: number;
  title: string;
  release_date: string;
  poster_path: string;
  vote_average: number;
}

export interface MovieDetails extends Movie {
  vote_count: number;
  overview: string;
  original_language: string;
}
