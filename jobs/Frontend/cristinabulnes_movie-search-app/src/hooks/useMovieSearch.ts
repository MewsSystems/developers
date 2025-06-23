import { useCallback, useEffect } from "react";
import { fetchMovies } from "../services/movieService";
import { useDebounce } from "./useDebounce";
import { useSearchMovieContext } from "../contexts/SearchMovieContext";

export const useMovieSearch = () => {
	const {
		movies,
		setMovies,
		isLoading,
		setIsLoading,
		error,
		setError,
		page,
		setPage,
		query,
		setQuery,
		hasMore,
		setHasMore,
	} = useSearchMovieContext();

	const debouncedQuery = useDebounce(query, 500);

	// Function to fetch movies
	const fetchAndSetMovies = useCallback(async (query: string, page: number) => {
		try {
			setIsLoading(true);
			setError(null);
			const response = await fetchMovies(query, page);

			setMovies((prev) =>
				page === 1 ? response.results : [...prev, ...response.results]
			);
			setHasMore(response.page < response.total_pages);
		} catch (err) {
			setError("Failed to load movies.");
		} finally {
			setIsLoading(false);
		}
	}, []);

	useEffect(() => {
		if (!debouncedQuery) {
			setMovies([]);
			setHasMore(false);
			return;
		}
		fetchAndSetMovies(debouncedQuery, page);
	}, [debouncedQuery, page, fetchAndSetMovies]);

	const loadMore = useCallback(() => {
		setPage((prevPage) => prevPage + 1);
	}, []);

	return {
		query,
		setQuery,
		movies,
		isLoading,
		error,
		hasMore,
		loadMore,
	};
};
