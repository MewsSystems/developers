import { useQuery } from "@tanstack/react-query";
import { queryMovieDetails } from "src/lib/api";

const useMovieDetails = (movieId: string) => {
  const { data, error, isLoading, isFetching } = useQuery({
    queryKey: ["movieDetails", movieId],
    queryFn: () => queryMovieDetails(movieId),
    enabled: !!movieId,
  });

  return {
    data,
    error,
    isLoading,
    isFetching,
  };
};

export default useMovieDetails;
