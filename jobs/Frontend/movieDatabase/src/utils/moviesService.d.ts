import { MovieDetails, MovieListItem } from "../types/movies";
export declare function getMoviesByTitle(searchTitle: string): Promise<MovieListItem[]>;
export declare function getMovieById(id: number): Promise<MovieDetails>;
