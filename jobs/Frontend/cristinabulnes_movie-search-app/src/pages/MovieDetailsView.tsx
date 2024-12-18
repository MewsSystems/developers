import { useNavigate, useParams } from "react-router-dom";
import MovieDetailsCard from "../components/MovieDetailsCard";
import { useEffect, useState } from "react";
import { Movie } from "../types";
import { fetchMovieDetails } from "../services/movieService";

const MovieDetailsView = () => {
	const { movieId } = useParams<{ movieId: string }>();

	const navigate = useNavigate();

	const [movieDetails, setMovieDetails] = useState<Movie>();
	const [loading, setLoading] = useState(false);
	const [error, setError] = useState<string | null>(null);

	const handleGoBack = () => navigate(-1);

	useEffect(() => {
		const getMovieDetails = async () => {
			setLoading(true);
			setError(null);

			try {
				if (movieId) {
					const details = await fetchMovieDetails(movieId);
					setMovieDetails(details);
				}
			} catch (err: any) {
				setError(err.message || "Failed to fetch movie details.");
			} finally {
				setLoading(false);
			}
		};

		getMovieDetails();
	}, [movieId]);

	if (loading) return <div>Loading...</div>;
	if (error) return <div style={{ color: "red" }}>{error}</div>;
	if (!movieDetails) return <div>No movie details available.</div>;

	const {
		title,
		posterPath,
		releaseDate,
		overview,
		genres,
		voteAverage: rating,
	} = movieDetails;

	return (
		<MovieDetailsCard
			title={title}
			posterPath={posterPath}
			releaseDate={releaseDate}
			overview={overview}
			genres={genres}
			rating={rating}
			onGoBack={handleGoBack}
		/>
	);
};

export default MovieDetailsView;
