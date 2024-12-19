import React from "react";
import Input from "../components/Input/Input";
import MoviesGrid from "../components/MoviesGrid";
import Loading from "../components/Loading";
import ErrorComponent from "../components/Error";
import { useMovieSearchRedux } from "../hooks/useMovieSearchRedux";

const SearchMoviesView = () => {
	const {
		query,
		handleInputChange,
		movies,
		isLoading,
		error,
		handleLoadMore,
		hasMore,
	} = useMovieSearchRedux();

	return (
		<>
			<Input
				id="search-movie-input"
				name="search-movie-input"
				placeholder="Search for a movie"
				label="Search for a movie"
				value={query}
				onChange={handleInputChange}
			/>
			{isLoading && <Loading />}
			{error && <ErrorComponent message={error} />}
			{!error && !isLoading && movies.length > 0 && (
				<MoviesGrid
					movies={movies}
					loadMore={handleLoadMore}
					hasMore={hasMore}
				/>
			)}
		</>
	);
};

export default React.memo(SearchMoviesView);
