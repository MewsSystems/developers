import { MovieType } from "src/store/types/MovieType";

export interface MovieProps {
  movie: MovieType;
  imgSize?: "200" | "300";
  disableLink?: boolean;
}
