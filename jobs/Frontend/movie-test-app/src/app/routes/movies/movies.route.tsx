import { useInfiniteMovies } from '../../api/movies.ts';
import { useContext, useEffect } from 'react';
import { useElementVisible } from '../../../hooks/useElementVisible.ts';
import MovieCard from '../../../components/movie-card';
import { GlobalContext } from '../../Provider.tsx';
import { MoviesGridContainer } from './movies.styles.tsx';
import { Header, HeaderPlaceholder } from '../../../components/header';
import Spinner from '../../../components/spinner';
import useDebounce from '../../../hooks/useDebouncer.ts';
import useMediaQuery from '../../../hooks/useMediaQuery.ts';
import { useTheme } from 'styled-components';

export const MoviesRoute = () => {
  const { searchQuery, setSearchQuery } = useContext(GlobalContext);
  const { visible: isLastElementVisible, setRef } = useElementVisible();

  const moviesQuery = useInfiniteMovies({ searchParam: searchQuery || '' });
  const movies = moviesQuery.data?.pages?.flatMap((page) => page.data.results) || [];
  //there are some duplicate movies in the response, so we need to filter them out
  const uniqueMovies = Array.from(new Set(movies.map((movie) => movie.id))).map((id) =>
    movies.find((movie) => movie.id === id),
  );

  const theme = useTheme();
  const isMobile = useMediaQuery(`(max-width: ${theme.breakpoints.tablet})`);

  //const navigate = useNavigate();

  const debouncedFunction = useDebounce((newSearchQuery) => {
    setSearchQuery(newSearchQuery);
  }, 500);

  useEffect(() => {
    if (isLastElementVisible && !moviesQuery.isFetchingNextPage && moviesQuery.hasNextPage) {
      moviesQuery.fetchNextPage();
    }
  }, [isLastElementVisible, moviesQuery]);

  if (moviesQuery.isError) {
    return <div>Something went wrong</div>;
  }

  if (moviesQuery.isLoading) {
    return <Spinner />;
  }

  return (
    <>
      <Header handleUpdateSearchQuery={debouncedFunction} searchQuery={searchQuery} isMobile={isMobile} />
      <HeaderPlaceholder /> {/* Placeholder for the header to have the same height*/}
      <MoviesGridContainer data-testid={'movies-container'}>
        {uniqueMovies.map((movie, index) => (
          <div
            data-testid={`movie-card-${movie?.id.toString()}`}
            key={movie?.id.toString()}
            ref={(instance) => (index === uniqueMovies.length - 1 ? setRef(instance) : null)}
          >
            {!!movie && <MovieCard movie={movie}></MovieCard>}
          </div>
        ))}
      </MoviesGridContainer>
    </>
  );
};
