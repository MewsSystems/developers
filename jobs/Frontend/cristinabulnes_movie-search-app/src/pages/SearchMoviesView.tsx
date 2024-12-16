import { mockMovies } from "../__mocks__/mockMovies";
import Input from "../components/Input";
import MoviesGrid from "../components/MoviesGrid";

const SearchMoviesView = () => {
	return (
		<>
			<div>My search movies component</div>
			<Input />
			<MoviesGrid movies={mockMovies} />
		</>
	);
};

export default SearchMoviesView;
