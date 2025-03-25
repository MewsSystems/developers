import { useCallback, useEffect, useState } from "react";
import type { useRouter } from "next/navigation";
import { api } from "~/trpc/react";

export function useMovieNavigation(
  router: ReturnType<typeof useRouter>,
  currentPage: number,
) {
  const [isNavigating, setIsNavigating] = useState(false);
  const [selectedMovieId, setSelectedMovieId] = useState<number | null>(null);

  /*
    Pre-fetch movie details query
  */
  const { data: movieDetails, isLoading: isLoadingDetails } =
    api.movie.getDetails.useQuery(
      { id: selectedMovieId ?? 0 },
      { enabled: !!selectedMovieId },
    );

  /*
    Handle navigation after data is loaded
  */
  useEffect(() => {
    if (selectedMovieId && movieDetails && !isLoadingDetails) {
      setIsNavigating(false);
      // Store the current page in sessionStorage before navigating
      sessionStorage.setItem("movieListPage", currentPage.toString());
      router.push(`/movie/${selectedMovieId}`);
    }
  }, [movieDetails, isLoadingDetails, selectedMovieId, router, currentPage]);

  const handleMovieClick = useCallback((movieId: number) => {
    setIsNavigating(true);
    setSelectedMovieId(movieId);
  }, []);

  return { isNavigating, handleMovieClick };
}
