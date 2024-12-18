import { useCallback, useEffect, useState } from "react";
import { fetchMovies } from "../services/movieService";
import { Movie } from "../types";
import { useDebounce } from "./useDebounce";

export const useMovieSearch = () => {
	const [movies, setMovies] = useState<Movie[]>([]);
	const [isLoading, setIsLoading] = useState(false);
	const [error, setError] = useState<string | null>(null);
	const [page, setPage] = useState(1);
	const [hasMore, setHasMore] = useState(true);
	const [query, setQuery] = useState("");

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
		movies,
		isLoading,
		error,
		hasMore,
		query,
		setQuery,
		loadMore,
	};
};
