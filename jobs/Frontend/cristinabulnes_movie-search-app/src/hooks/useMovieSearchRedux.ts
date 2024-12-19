import { loadMore, setQuery } from "../redux/slices/movieSearchSlice";
import { useDispatch, useSelector } from "react-redux";
import { AppDispatch, RootState } from "../redux/store";
import { fetchMoviesAsync } from "../redux/thunk/movieSearchThunk";
import { useCallback, useEffect } from "react";
import { useDebounce } from "./useDebounce";

export const useMovieSearchRedux = () => {
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

	return {
		query,
		handleInputChange,
		movies,
		isLoading,
		error,
		handleLoadMore,
		hasMore,
	};
};
