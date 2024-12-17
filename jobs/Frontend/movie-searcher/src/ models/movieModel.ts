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

export type Movie = {
  id: number;
  poster: string | null;
  release_date: string;
  title: string;
};

export type MovieReponse = {
  page: number;
  totalPages: number;
  movies: Movie[];
};
