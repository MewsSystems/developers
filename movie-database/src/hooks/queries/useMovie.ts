import { useQuery } from "@tanstack/react-query";
import { apiFetch, ErrorReason, throwError } from "../../api/base";
import { movieSchema, Movie } from "@/api/schemas/movieSchema";

const fetchMovie = async (id: number) => {
  const response = await apiFetch<Movie>(`movie/${id}`);
  const parsedResponse = movieSchema.safeParse(response);
  return parsedResponse.success ? parsedResponse.data : throwError('invalid-response');
};

export const useMovie = (id: number) => {
  return useQuery<Movie, ErrorReason>({
    queryKey: ["movie", id],
    queryFn: () => fetchMovie(id),
  });
};
