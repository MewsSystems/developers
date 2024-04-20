import React, { useState } from "react";
import { domainURL } from "./utils/constant";
import { MovieDetailResponse } from "./types/movies";
import { useSpinDelay } from "spin-delay";

export const useMovieDetail = (id: number) => {
  const [movieDetail, setMovieDetail] =
    React.useState<MovieDetailResponse | null>(null);
  const [isLoading, setLoading] = useState(false);
  const showIsLoading = useSpinDelay(isLoading, {
    delay: 500,
    minDuration: 200,
  });

  const getDetail = async () => {
    try {
      setLoading(true);
      const fetchURL = `${domainURL}movie/${id}?api_key=${
        import.meta.env.VITE_TMDB_KEY
      }`;
      const res = await fetch(fetchURL);
      const data = await res.json();

      setMovieDetail(data);
      setLoading(false);
    } catch (error) {
      setMovieDetail(null);
      setLoading(false);
    }
  };

  React.useEffect(() => {
    getDetail();
  }, [id]);

  return { movieDetail, isLoading: showIsLoading };
};
