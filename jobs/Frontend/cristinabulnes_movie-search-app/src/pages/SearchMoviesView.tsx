import React, { useCallback, useEffect } from "react";
import { useDispatch, useSelector } from "react-redux";
import { AppDispatch, RootState } from "../redux/store";
import { fetchMoviesAsync } from "../redux/thunk/movieSearchThunk";
import { loadMore, setQuery } from "../redux/slices/movieSearchSlice";
import { useDebounce } from "../hooks/useDebounce";
import Input from "../components/Input/Input";
import MoviesGrid from "../components/MoviesGrid";
import Loading from "../components/Loading";
import ErrorComponent from "../components/Error";

const SearchMoviesView = () => {
	const handleInputChange = useCallback(
		(e: React.ChangeEvent<HTMLInputElement>) => {
			dispatch(setQuery(e.target.value));
		},
		[]
	);

	const dispatch = useDispatch<AppDispatch>();
	const { movies, isLoading, error, hasMore, query, page } = useSelector(
		(state: RootState) => state.movieSearch
	);

	const debouncedQuery = useDebounce(query, 500);

	const fetchMovies = useCallback(async () => {
		if (debouncedQuery) {
			dispatch(fetchMoviesAsync({ query: debouncedQuery, page }));
		}
	}, [dispatch, debouncedQuery, page]);

	useEffect(() => {
		if (debouncedQuery) {
			fetchMovies();
		}
	}, [debouncedQuery, page, fetchMovies]);

	const handleLoadMore = () => {
		dispatch(loadMore());
	};

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
