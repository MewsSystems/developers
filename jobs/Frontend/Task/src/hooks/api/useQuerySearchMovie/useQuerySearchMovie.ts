import { useQuery } from "@tanstack/react-query";
import { API_KEY, BASE_URL } from "../../../constants";
import { SearchMovieQuery, SearchMovieResponse } from "../../../types";

type useQuerySearchMovieProps = {
  query: string;
  page: number;
}

const fetchSearchMovie = async ({ query, page }: SearchMovieQuery): Promise<SearchMovieResponse> => {
  const response = await fetch(`${BASE_URL}/search/movie?api_key=${API_KEY}&query=${query}&page=${page}`);

  if (!response.ok) {
    throw new Error();
  }

  return response.json();
};

export const useQuerySearchMovie = ({ query, page }: useQuerySearchMovieProps) => {
  const { data, isLoading, isError } = useQuery({
    queryFn: () => fetchSearchMovie({ query, page }),
    queryKey: ["search-movies", query, page],
    placeholderData: (previousData) => previousData,
  });
  
  return {
    data,
    isLoading,
    isError
  };
};
