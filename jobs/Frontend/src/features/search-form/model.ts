import { Movie } from "../../models/movie";

export interface Props {
    readonly keyword: string;
    readonly setKeyword: React.Dispatch<React.SetStateAction<string>>;
    readonly movies: Movie[];
    readonly setPage: (page: number) => void;
    readonly page: number;
    readonly totalPages: number;
};