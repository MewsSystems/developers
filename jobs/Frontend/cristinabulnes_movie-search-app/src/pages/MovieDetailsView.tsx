import { useNavigate, useParams } from "react-router-dom";
import Button from "../components/Button";
import MovieCard from "../components/BaseMovieCard";
import { mockMovies } from "../__mocks__/mockMovies";

const MovieDetailsView = () => {
	const { movieId } = useParams<{ movieId: string }>();

	const navigate = useNavigate();

	const handleGoBack = () => navigate(-1);

	const { title, posterPath, releaseDate, rating } = mockMovies[0];

	return (
		<>
			<div>My movie details component</div>
			<MovieCard {...{ title, posterPath, releaseDate, rating }} />
			<Button onClick={handleGoBack}>Go back!</Button>
		</>
	);
};

export default MovieDetailsView;
