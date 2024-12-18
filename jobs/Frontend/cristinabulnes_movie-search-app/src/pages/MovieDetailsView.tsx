import { useNavigate, useParams } from "react-router-dom";
import MovieDetailsCard from "../components/MovieDetailsCard";
import { useMovieDetails } from "../hooks/useMovieDetails";

const MovieDetailsView = () => {
	const { movieId } = useParams<{ movieId: string }>();
	const navigate = useNavigate();

	const { movieDetails, loading, error } = useMovieDetails(movieId);

	const handleGoBack = () => navigate(-1);

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
