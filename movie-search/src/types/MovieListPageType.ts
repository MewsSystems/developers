import {MovieType} from "./MovieType.ts";

export type MovieListPageType = {
    page: number
    results: MovieType[]
    total_pages: number
    total_results: number
}