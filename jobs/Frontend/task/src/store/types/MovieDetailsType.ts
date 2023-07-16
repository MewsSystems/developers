import { MovieType } from "src/store/types/MovieType";

export interface MovieDetailsType extends MovieType {
  genres: { id: string; name: string }[];
  imdb_id: string;
}
