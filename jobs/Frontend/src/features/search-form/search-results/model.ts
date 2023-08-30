import { Movie } from "../../../models/movie";

export interface Props {
    readonly movies: Movie[];
    readonly page: number;
    readonly totalPages: number;
}