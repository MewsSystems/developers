import { useInfiniteMovies } from '../../api/movies.ts';
import { useContext, useEffect } from 'react';
import { useElementVisible } from '../../../hooks/useElementVisible.ts';
import MovieCard from '../../../components/movie-card/index.tsx';
import { GlobalSearchContext } from '../../Provider.tsx';
import { MoviesGridContainer } from './movies.styles.tsx';

export const MoviesRoute = () => {
  const { searchQuery } = useContext(GlobalSearchContext);
  const { visible: isLastElementVisible, setRef } = useElementVisible();

  const moviesQuery = useInfiniteMovies({ searchParam: searchQuery || '' });
  const movies = moviesQuery.data?.pages.flatMap((page) => page.data.results) || [];
  //there are some duplicate movies in the response, so we need to filter them out
  const uniqueMovies = Array.from(new Set(movies.map((movie) => movie.id))).map((id) =>
    movies.find((movie) => movie.id === id),
  );

  useEffect(() => {
    if (isLastElementVisible && !moviesQuery.isFetchingNextPage && moviesQuery.hasNextPage) {
      moviesQuery.fetchNextPage();
    }
  }, [isLastElementVisible, moviesQuery]);

  return (
    <MoviesGridContainer>
      {uniqueMovies.map((movie, index) => (
        <div
          key={movie?.id.toString()}
          ref={(instance) => (index === uniqueMovies.length - 1 ? setRef(instance) : null)}
        >
          {!!movie && <MovieCard movie={movie}></MovieCard>}
        </div>
      ))}
    </MoviesGridContainer>
  );
};
