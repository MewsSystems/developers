import { domainURL } from "../utils/constant";
import { MovieDetailResponse } from "../types/movies";
import { useQuery } from "@tanstack/react-query";
import { customFetch } from "../utils/customFetch";

export const useMovieDetail = (id: number) => {
  const getDetail = async () => {
    const fetchURL = `${domainURL}movie/${id}?api_key=${
      import.meta.env.VITE_TMDB_KEY
    }`;
    return await customFetch<MovieDetailResponse>(fetchURL);
  };

  const { data, isFetching, isLoading, status, error } = useQuery({
    queryKey: ["movieDetail", { id }],
    queryFn: getDetail,
  });

  return { movieDetail: data, isLoading, isFetching, status, error };
};
