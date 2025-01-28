import { Genre } from "./genreTypes";

export interface Movie {
    id: number;
    title: string;
    overview: string;
    release_date: string;
    poster_path: string;
    vote_average: number;
    runtime: number;
    genres: Genre[];
}

export interface MovieListProps {
    movies: Movie[];
    loading?: boolean;
    searchPerformed: boolean;
}