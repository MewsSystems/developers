import {useInfiniteQuery} from "@tanstack/react-query"
import {fetchMovies} from "./fetchMovies.ts"
import {MovieListPageType} from "../types/MovieListPageType.ts";

export const useMovies = (query: string) => {
    return useInfiniteQuery<MovieListPageType>({
        queryKey: ["movies", query],
        queryFn: ({pageParam}) => fetchMovies({query, pageParam: pageParam as number}),
        initialPageParam: 1,
        getNextPageParam: (lastPage) => {
            if (lastPage.page >= lastPage.total_pages) return undefined
            return lastPage.page + 1
        },
        enabled: query.length >= 3,
    })
}