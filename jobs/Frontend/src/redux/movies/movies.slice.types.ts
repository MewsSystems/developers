// Define a type for the slice state
export type Movie = {
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
  title: string;
  video: boolean;
  vote_average: number;
  vote_count: number;
};

export type MovieSearch = {
  results: Movie[];
  page: number;
  total_pages: number | null;
};

export interface MoviesState {
  search: MovieSearch;
  query: string;
  selectedMovie: Movie | null;
}
