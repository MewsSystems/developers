import { useNavigate, useParams } from "react-router-dom";
import styled from "styled-components";
import { useMovieDetails } from "../hooks/useMovieDetails";
import MovieDetailsCard from "../components/MovieDetailsCard";

const ModalOverlay = styled.div`
	position: fixed;
	top: 0;
	left: 0;
	width: 100%;
	height: 100%;
	background: black;
	display: flex;
	align-items: center;
	justify-content: center;
	z-index: 1000;
	transition: opacity 0.3s ease-in-out;
`;

const ModalContent = styled.div`
	background: black;
	margin: auto;
	border-radius: 8px;
	padding: 20px;
	width: 100vw;
	box-shadow: 0 4px 6px rgba(0, 0, 0, 0.1);
`;

const MovieDetails = () => {
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
		<ModalOverlay onClick={handleGoBack} aria-modal="true" role="dialog">
			<ModalContent onClick={(e) => e.stopPropagation()}>
				<MovieDetailsCard
					title={title}
					posterPath={posterPath}
					releaseDate={releaseDate}
					overview={overview}
					genres={genres}
					rating={rating}
					onGoBack={handleGoBack}
				/>
			</ModalContent>
		</ModalOverlay>
	);
};

export default MovieDetails;
