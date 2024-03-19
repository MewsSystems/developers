import { useQuery } from "@tanstack/react-query";
import { API_KEY, BASE_URL } from "../../../constants";
import { MovieDetailsResponse } from "../../../types";

type MovieDetailsParams = {
  id: number;
}

const fetchMovieDetails = async ({ id }: MovieDetailsParams): Promise<MovieDetailsResponse> => {
  const response = await fetch(`${BASE_URL}/movie/${id}?api_key=${API_KEY}`);

  if (!response.ok) {
    throw new Error();
  }

  return response.json();
};

export const useQueryMovieDetails = ({ id }: MovieDetailsParams) => {
  const { data, isLoading, isError } = useQuery({
    enabled: !!id,
    queryFn: () => fetchMovieDetails({ id }),
    queryKey: ["movie-details", id]
  });

  return {
    data,
    isLoading,
    isError
  };
};
