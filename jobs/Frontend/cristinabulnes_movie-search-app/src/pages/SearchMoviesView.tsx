import { useCallback, useEffect, useState } from "react";
import Input from "../components/Input/Input";
import MoviesGrid from "../components/MoviesGrid";
import { fetchMovies } from "../services/movieService";
import { Movie } from "../types";
import Button from "../components/Button";

const SearchMoviesView = () => {
	const inputId = "search-movie-input";
	const [value, setValue] = useState("");
	const [movies, setMovies] = useState<Movie[]>([]);
	const [isLoading, setIsLoading] = useState(false);
	const [error, setError] = useState<string | null>(null);
	const [page, setPage] = useState(1);
	const [hasMore, setHasMore] = useState(true);

	const handleSearch = useCallback(
		async (query: string, pageNumber: number) => {
			try {
				setIsLoading(true);
				setError(null);
				const response = await fetchMovies(query, pageNumber);

				setMovies((prev) =>
					pageNumber === 1 ? response.results : [...prev, ...response.results]
				);
				setHasMore(response.page < response.total_pages);
			} catch (err) {
				setError("Failed to load movies.");
			} finally {
				setIsLoading(false);
			}
		},
		[]
	);

	// Debounce input changes
	useEffect(() => {
		const debounceTimeout = setTimeout(() => {
			setPage(1); // Reset to the first page on new search
			handleSearch(value, 1);
		}, 500); // Debounce delay of 500ms

		return () => clearTimeout(debounceTimeout);
	}, [value, handleSearch]);

	// Handle pagination changes
	useEffect(() => {
		if (page > 1) {
			handleSearch(value, page);
		}
	}, [page, handleSearch]);

	// Handle "Load More" button click
	const handleLoadMore = useCallback(() => {
		setPage((prevPage) => prevPage + 1);
	}, []);

	return (
		<>
			<Input
				id={inputId}
				name={inputId}
				placeholder="Search for a movie"
				label="Search for a movie"
				value={value}
				onChange={(e) => setValue(e.target.value)}
			/>
			{isLoading && <div>Loading...</div>}
			{error && <div style={{ color: "red" }}>{error}</div>}
			{movies.length > 0 && (
				<>
					<MoviesGrid movies={movies} />
					{hasMore && <Button onClick={handleLoadMore}>Load More</Button>}
				</>
			)}
		</>
	);
};

export default SearchMoviesView;
