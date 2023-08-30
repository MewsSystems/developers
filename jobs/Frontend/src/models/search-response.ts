import { Movie } from "./movie";

export interface SearchResponse {
    readonly page: number;
    readonly total_results: number;
    readonly total_pages: number;
    readonly results: Movie[];
}