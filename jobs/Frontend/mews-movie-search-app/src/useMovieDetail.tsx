import React from "react";
import { domainURL } from "./utils/constant";
import { MovieDetailResponse } from "./types/movies";

export const useMovieDetail = (id: number) => {
  const [movieDetail, setMovieDetail] =
    React.useState<MovieDetailResponse | null>(null);

  const getDetail = async () => {
    try {
      const fetchURL = `${domainURL}movie/${id}?api_key=${
        import.meta.env.VITE_TMDB_KEY
      }`;
      const res = await fetch(fetchURL);
      const data = await res.json();

      setMovieDetail(data);
    } catch (error) {
      setMovieDetail(null);
    }
  };

  React.useEffect(() => {
    getDetail();
  }, [id]);

  return { movieDetail };
};
