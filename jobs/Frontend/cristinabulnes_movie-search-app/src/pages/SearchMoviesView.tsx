import { useState } from "react";
import { mockMovies } from "../__mocks__/mockMovies";
import Input from "../components/Input/Input";
import MoviesGrid from "../components/MoviesGrid";

const SearchMoviesView = () => {
	const inputId = "search-movie-input";
	const [value, setValue] = useState("");

	return (
		<>
			<div>My search movies component</div>
			<Input
				id={inputId}
				name={inputId}
				placeholder="Search for a movie"
				label="Search for a movie"
				value={value}
				onChange={(e) => setValue(e.target.value)}
			/>
			<MoviesGrid movies={mockMovies} />
		</>
	);
};

export default SearchMoviesView;
