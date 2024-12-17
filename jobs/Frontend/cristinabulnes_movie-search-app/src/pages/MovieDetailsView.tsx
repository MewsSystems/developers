import { useNavigate, useParams } from "react-router-dom";
import { mockMovies } from "../__mocks__/mockMovies";
import MovieDetailsCard from "../components/MovieDetailsCard";

const MovieDetailsView = () => {
	const { movieId } = useParams<{ movieId: string }>();

	const navigate = useNavigate();

	const handleGoBack = () => navigate(-1);

	const {
		id,
		title,
		posterPath,
		releaseDate,
		overview,
		genres,
		voteAverage: rating,
	} = mockMovies[0];

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
