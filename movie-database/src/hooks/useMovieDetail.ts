import { useQuery } from "@tanstack/react-query";
import { apiFetch } from "../api/base";
import { movieSchema, Movie } from "@/api/schemas/movieSchema";

const fetchMovieDetail = async (id: number) => {
  const response = await apiFetch<Movie>(`movie/${id}`);
  return movieSchema.parse(response);
};

export const useMovieDetail = (id: number) => {
  return useQuery<Movie>({
    queryKey: ["movieDetail", id],
    queryFn: () => fetchMovieDetail(id),
  });
};
