import MovieSearchHeader from './components/MovieSearchHeader/MovieSearchHeader';
import MoviesList from './components/MoviesList/MoviesList';
import useDiscoverMovies from './hooks/useDiscoverMovies';
import useSearchMovies from './hooks/useSearchMovies';

const Movies = () => {
    const { discoverMovies } = useDiscoverMovies();

    const {
        moviesData,
        searchQuery,
        pageNumber,
        totalPages,
        isLoading,
        showSearchMovieResults,
        showPagination,
        handleSearchQueryChange,
        handlePageNumberChange,
    } = useSearchMovies();

    return (
        <div>
            <MovieSearchHeader
                searchQuery={searchQuery}
                onSearchChange={handleSearchQueryChange}
                currentPageNumber={pageNumber}
                totalPages={totalPages}
                showPagination={showPagination}
                onPageChange={handlePageNumberChange}
                isLoading={isLoading}
            />
            {!showSearchMovieResults && !discoverMovies && (
                <div>No results found.</div>
            )}
            {!showSearchMovieResults && discoverMovies && (
                <MoviesList movies={discoverMovies.results ?? []} />
            )}
            {showSearchMovieResults && (
                <MoviesList movies={moviesData?.results ?? []} />
            )}
        </div>
    );
};

export default Movies;
