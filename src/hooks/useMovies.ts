import { keepPreviousData, useQuery } from "@tanstack/react-query"
import { movieService } from "../services/movieService"

export const usePopularMovies = (page = 1) => {
  return useQuery({
    queryKey: ["movies", "popular", page],
    queryFn: () => movieService.getPopularMovies(page),
    placeholderData: keepPreviousData,
    staleTime: 10 * 60 * 1000,
  })
}

export const useMovieSearch = (query: string, page = 1) => {
  return useQuery({
    queryKey: ["movies", "search", query, page],
    queryFn: () => movieService.searchMovies(query, page),
    enabled: !!query.trim(),
    placeholderData: keepPreviousData,
    staleTime: 5 * 60 * 1000,
  })
}

export const useMovieDetails = (id: number) => {
  return useQuery({
    queryKey: ["movies", "detail", id],
    queryFn: () => movieService.getMovieById(id),
    enabled: id > 0 && !Number.isNaN(id),
    staleTime: 10 * 60 * 1000,
  })
}
