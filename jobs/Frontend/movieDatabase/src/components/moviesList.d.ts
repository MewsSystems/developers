import { FC } from "react";
import './moviesList.less';
import { MovieListItem } from "../types/movies";
interface MoviesListProps {
    movies?: MovieListItem[];
    setSelectedMovieId?(id: number): any;
}
export declare const MoviesList: FC<MoviesListProps>;
export {};
