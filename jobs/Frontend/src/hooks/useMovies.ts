import { useQuery } from "@tanstack/react-query";
import { queryMovies } from "src/lib/api";

const useMovies = (query: string, page: string) => {
  const { data, error, isLoading, isFetching } = useQuery({
    queryKey: ["movies", query, page],
    queryFn: () => queryMovies(query, page),
    enabled: !!query,
    staleTime: 10000, // can be set to whatever makes sense given the context
  });

  return {
    data,
    error,
    isLoading,
    isFetching,
  };
};

export default useMovies;
