import { useEffect, useState, useCallback } from "react";
import { apiKey } from "../config/secrets";
import { MovieDetail } from "../types/types";

const useGetMovieDetail = (id: string) => {
  const [data, setData] = useState<null | MovieDetail>(
    null
  );
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState(null);

  const fetchMovie = useCallback(async () => {
    setLoading(true);
    try {
      const response = await fetch(
        `https://api.themoviedb.org/3/movie/${id}?api_key=${apiKey}`
      );
      const data = await response.json();
      setData(data);
    } catch (error: any) {
      setError(error);
    }
    setLoading(false);
  }, [id]);

  useEffect(() => {
    if (id !== "") {
      fetchMovie();
    }
  }, [id, fetchMovie]);

  return { data, loading, error };
};

export default useGetMovieDetail;
