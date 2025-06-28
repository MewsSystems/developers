export type Movie = {
  id: number;
  adult: boolean;
  backdrop_path: string | null;
  genre_ids: number[];
  original_language: string;
  original_title: string;
  overview: string;
  poster_path: string | null;
  popularity: number;
  release_date: string;
  title: string;
  video: boolean;
  vote_average: number;
  vote_count: number;
};

export type PopularMoviesResponse = {
  page: number;
  results: Movie[];
  totalPages: number;
  totalResults: number;
};
