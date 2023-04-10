export interface Movie {
  original_title: string;
  poster_path: string;
  genre_ids: number[];
  vote_average: number;
  vote_count: number;
  [x: string]: string | number | boolean | number[] | string[];
}
