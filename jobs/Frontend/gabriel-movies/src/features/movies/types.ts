export type Movie = {
  id: number;
  title: string;
  posterPath: string;
  releaseDate: string;
  formattedReleaseDate: string;
  overview: string;
  popularity: number;
  voteAverage: number;
};

export type TmdbMovie = {
  id: number;
  title: string;
  poster_path: string;
  release_date: string;
  overview: string;
  popularity: number;
  vote_average: number;
}

export type TmdbSearchResponse = {
  page: number;
  total_pages: number;
  total_results: number;
  results: TmdbMovie[];
};

export type TmdbMovieDetailsResponse = TmdbMovie;