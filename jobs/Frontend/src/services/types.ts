export type MovieDetailResponse = {
  id: number;
  title: string;
  overview: string;
  poster_path: string;
};

export type MovieServiceRespnse = {
  page: number;
  total_pages: number;
  results: Array<MovieDetailResponse>;
};