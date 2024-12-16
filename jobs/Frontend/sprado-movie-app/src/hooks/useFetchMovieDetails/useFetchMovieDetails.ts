import { useState, useEffect } from "react";
import { Movie } from "../../types";

interface UseMovieFetchResult {
  movie: Movie | null;
  isLoading: boolean;
  error: Error | null;
}

export const useFetchMovieDetails = (id: string | undefined): UseMovieFetchResult => {
  const [movie, setMovie] = useState<Movie | null>(null);
  const [isLoading, setIsLoading] = useState<boolean>(false);
  const [error, setError] = useState<Error | null>(null);

  useEffect(() => {
    const fetchMovieDetail = async () => {
      if (!id) return;
      setIsLoading(true);
      setError(null);

      try {
        const response = await fetch(
          `https://api.themoviedb.org/3/movie/${id}?api_key=${process.env.REACT_APP_TMDB_API_KEY}&language=en-US`
        );
        if (!response.ok) {
          throw new Error("Failed to fetch movie details");
        }

        const data = await response.json();
        setMovie(data);
      } catch (err: any) {
        setError(err);
      } finally {
        setIsLoading(false);
      }
    };

    fetchMovieDetail();
  }, [id]);

  return { movie, isLoading, error };
};
