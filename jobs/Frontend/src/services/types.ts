export type MovieDetailResponse = {
  id: number;
  title: string;
  overview: string;
  poster_path: string;
  homepage: string;
  genres: Array<{id: number; name: string}>;
  spoken_languages: Array<{english_name: string; iso_639_1: string; name: string}>;
};

export type MovieSummaryResponse = {
  id: number;
  title: string;
  overview: string;
  poster_path: string;
};

export type MovieServiceRespnse = {
  page: number;
  total_pages: number;
  results: Array<MovieSummaryResponse>;
};