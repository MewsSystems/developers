import { Movie } from "../models/movie";

export interface SearchState {
    readonly status: 'idle' | 'loading' | 'failed';
    readonly keyword: string;
    readonly movies: Movie[];
    readonly page: number;
    readonly totalPages: number;
}