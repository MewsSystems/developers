import { GenreDto } from "../dtos/GenreDto";
import { MovieDto } from "../dtos/movieDto";

export interface InitialState {
  genres: GenreDto[];
  selectedMovie: MovieDto;
  selectedMovieGenres: string[];
}
