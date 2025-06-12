export interface Movie {
  id: number;
  title: string;
  overview: string;
  poster_path: string | null;
  release_date: string;
  vote_average: number;
  [key: string]: any;
}

export interface MoviesResponse {
  page: number;
  results: Movie[];
  total_pages: number;
  total_results: number;
}

export interface MovieDetailResponse extends Movie {
  genres: { id: number; name: string }[];
  runtime: number;
  tagline: string;
}
