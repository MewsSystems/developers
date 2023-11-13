export type Movie = {
  id: number;
  title: string;
  backdrop_path: string;
  adult: boolean;
  genre_ids: number[];
  original_language: string;
  overview: string;
  popularity: number;
  poster_path: string;
  release_date: string;
  video: boolean;
  vote_average: number;
  vote_count: number;
};
