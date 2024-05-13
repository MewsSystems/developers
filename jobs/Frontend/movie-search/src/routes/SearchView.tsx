import MovieList from '../components/MovieList/MovieList';
import { useMovies } from '../hooks/useMovies';
import SearchBox from '../components/MovieList/SearchBox';
import { Box } from '@mui/material';

const SearchView = () => {
	const { movies, query, setQuery, page, setPage, totalPages } = useMovies();

	return (
		<>
			<Box display='flex' justifyContent='center'>
				<SearchBox query={query} onSearch={setQuery} />
			</Box>
			{movies && (
				<MovieList movies={movies} page={page} setPage={setPage} totalPages={totalPages} />
			)}
		</>
	);
};

export default SearchView;
