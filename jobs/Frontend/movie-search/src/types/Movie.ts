export interface Movie {
  id: number;
  original_title: string;
  title: string;
  overview: string;
  adult: boolean;
  backdrop_path: string;
  genre_ids: number[];
  popularity: number;
  poster_path: string;
  release_date: string;
}
