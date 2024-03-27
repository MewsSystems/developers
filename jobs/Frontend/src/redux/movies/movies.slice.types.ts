// Define a type for the slice state
export type Movie = {
  id: number;
  title: string;
  year: string;
  genre: string;
  director: string;
  poster: string;
};

export interface MoviesState {
  results: Movie[];
  query: string;
  selectedMovie: Movie | null;
}
