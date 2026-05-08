import {useQuery} from "@tanstack/react-query";
import {MovieDetailsType} from "../types/MovieDetailsType.ts";
import {fetchMovieById} from "./fetchMovieById.ts";

export const useMovieDetails = (id: number) => {
    return useQuery<MovieDetailsType>({
        queryKey: ["movie", id],
        queryFn: () => fetchMovieById(id),
        enabled: !!id,
    })
}