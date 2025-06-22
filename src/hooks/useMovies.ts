import { useQuery } from "@tanstack/react-query"
import { movieService } from "../services/movieService"

export const usePopularMovies = (page = 1) => {
  return useQuery({
    queryKey: ["movies", "popular", page],
    queryFn: () => movieService.getPopularMovies(page),
    staleTime: 10 * 60 * 1000,
  })
}

export const useMovieSearch = (query: string, page = 1) => {
  return useQuery({
    queryKey: ["movies", "search", query, page],
    queryFn: () => movieService.searchMovies(query, page),
    enabled: !!query.trim(),
    staleTime: 5 * 60 * 1000,
  })
}

export const useMovieDetails = (id: number) => {
  return useQuery({
    queryKey: ["movies", "detail", id],
    queryFn: () => movieService.getMovieById(id),
    enabled: !!id,
    staleTime: 10 * 60 * 1000,
  })
}
