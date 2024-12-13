export interface Movie {
  title: string;
  adult: boolean;
  backdrop_path: string;
  genre_ids: number[];
  id: number;
  original_language: string;
  original_title: string;
  overview: string;
  popularity: number;
  poster_path: string;
  release_date: string;
  video: boolean;
  vote_average: number;
  vote_count: number;
  success?: boolean;
}

export interface DiscoverMovieResponse {
  results: Movie[];
  page: number;
  total_pages: number;
  total_results: number;
}
