import MovieList from '../components/MovieList/MovieList';
import { useMovies } from '../hooks/useMovies';
import SearchBox from '../components/Search/SearchBox';
import { Box } from '@mui/material';
import LoadingComponent from '../components/Loading/LoadingComponent';

const SearchView = () => {
	const { movies, query, setQuery, page, setPage, totalPages, loading } = useMovies();

	return (
		<div className='search-view'>
			<Box display='flex' justifyContent='center'>
				<SearchBox search-test={'search'} query={query} onSearch={setQuery} />
			</Box>
			{loading ? (
				<LoadingComponent />
			) : (
				movies && (
					<MovieList movies={movies} page={page} setPage={setPage} totalPages={totalPages} />
				)
			)}
		</div>
	);
};

export default SearchView;
