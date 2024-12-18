// hooks/useMovieDetails.ts
import { useState, useEffect } from "react";
import { fetchMovieDetails } from "../services/movieService";
import { Movie } from "../types";

export const useMovieDetails = (movieId: string | undefined) => {
	const [movieDetails, setMovieDetails] = useState<Movie | null>(null);
	const [loading, setLoading] = useState(false);
	const [error, setError] = useState<string | null>(null);

	useEffect(() => {
		const getMovieDetails = async () => {
			if (!movieId) {
				setError("No movie ID provided.");
				setLoading(false);
				return;
			}

			try {
				setLoading(true);
				const data = await fetchMovieDetails(movieId);
				setMovieDetails(data);
			} catch (err) {
				setError("Failed to load movie details. Please try again.");
			} finally {
				setLoading(false);
			}
		};

		getMovieDetails();
	}, [movieId]);

	return { movieDetails, loading, error };
};
