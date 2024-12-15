import { useNavigate, useParams } from "react-router-dom";
import Button from "../components/Button";
import MovieCard from "../components/MovieCard";

const MovieDetailsView = () => {
	const { movieId } = useParams<{ movieId: string }>();

	const navigate = useNavigate();

	const handleGoBack = () => navigate(-1);

	return (
		<>
			<div>My movie details component</div>
			<MovieCard />
			<Button onClick={handleGoBack}>Go back!</Button>
		</>
	);
};

export default MovieDetailsView;
