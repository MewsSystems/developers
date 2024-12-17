export type MovieTmdbApi = {
  adult: boolean;
  backdrop_path: string;
  genre_ids: number[];
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
};

export type MovieDetailsTmdbApi = {
  genres: { id: number; name: string }[];
  id: number;
  imdb_id: string;
  overview: string;
  poster_path: string;
  release_date: string;
  title: string;
};

export type Movie = {
  id: number;
  poster: string | null;
  title: string;
};

export type MovieDetails = {
  genres: string[];
  id: number;
  overview: string;
  poster: string;
  releaseDate: string;
  title: string;
};

export type MovieResponse = {
  page: number;
  totalPages: number;
  movies: Movie[];
};
