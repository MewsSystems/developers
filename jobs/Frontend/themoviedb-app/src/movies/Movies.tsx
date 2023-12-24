import MovieSearchInput from './components/MovieSearchInput';
import MoviesList from './components/MoviesList';
import Pagination from './components/Pagination';
import useSearchMovies from './hooks/useSearchMovies';

const Movies = () => {
    const {
        moviesData,
        searchQuery,
        pageNumber,
        isLoading,
        handleSearchQueryChange,
        handlePageNumberChange,
    } = useSearchMovies();

    return (
        <div>
            <MovieSearchInput
                value={searchQuery}
                onChange={handleSearchQueryChange}
            />
            {isLoading ? 'Loading...' : 'Not loading'}
            {moviesData && moviesData.total_pages > 1 && (
                <Pagination
                    page={pageNumber}
                    totalPages={moviesData && moviesData.total_pages}
                    onChange={handlePageNumberChange}
                />
            )}
            {!moviesData ||
                (moviesData.results.length === 0 && <div>No results.</div>)}
            {searchQuery && moviesData && (
                <MoviesList movies={moviesData?.results ?? []} />
            )}
        </div>
    );
};

export default Movies;
