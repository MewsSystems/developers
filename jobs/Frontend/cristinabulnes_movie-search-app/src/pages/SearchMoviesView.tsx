import { useCallback, useEffect, useState } from "react";
import Input from "../components/Input/Input";
import MoviesGrid from "../components/MoviesGrid";
import { fetchMovies } from "../services/movieService";
import { Movie } from "../types";

const SearchMoviesView = () => {
	const inputId = "search-movie-input";
	const [value, setValue] = useState("");
	const [movies, setMovies] = useState<Movie[]>();
	const [loading, setLoading] = useState(false);
	const [error, setError] = useState<string | null>(null);

	// Debounce handler for API call
	const handleSearch = useCallback(async () => {
		if (!value) return;

		setLoading(true);
		setError(null);
		try {
			const response = await fetchMovies(value, 1);
			setMovies(response.results);
		} catch (err: any) {
			setError(err.message || "Failed to fetch movies.");
		} finally {
			setLoading(false);
		}
	}, [value]);

	useEffect(() => {
		const debounceTimeout = setTimeout(() => {
			handleSearch();
		}, 500); // Debounce delay of 500ms

		return () => clearTimeout(debounceTimeout);
	}, [value, handleSearch]);

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
			{loading && <div>Loading...</div>}
			{error && <div style={{ color: "red" }}>{error}</div>}
			{movies && movies.length > 0 && <MoviesGrid movies={movies} />}
		</>
	);
};

export default SearchMoviesView;
