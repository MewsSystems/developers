import Input from "../components/Input/Input";
import MoviesGrid from "../components/MoviesGrid";
import Button from "../components/Button";
import { useMovieSearch } from "../hooks/useMovieSearch";

const SearchMoviesView = () => {
	const { movies, isLoading, error, hasMore, query, setQuery, loadMore } =
		useMovieSearch();

	return (
		<>
			<Input
				id="search-movie-input"
				name="search-movie-input"
				placeholder="Search for a movie"
				label="Search for a movie"
				value={query}
				onChange={(e) => setQuery(e.target.value)}
			/>
			{isLoading && <div>Loading...</div>}
			{error && <div style={{ color: "red" }}>{error}</div>}
			{movies.length > 0 && (
				<>
					<MoviesGrid movies={movies} loadMore={loadMore} hasMore={hasMore} />
					{hasMore && <Button onClick={loadMore}>Load More</Button>}
				</>
			)}
		</>
	);
};

export default SearchMoviesView;
