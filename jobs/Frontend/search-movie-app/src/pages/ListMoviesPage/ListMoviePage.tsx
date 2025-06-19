import { memo, useMemo } from 'react';
import {
  RenderErrorLayout,
  RenderLoader,
  RenderMovieList,
  RenderResultNotFoundLayout,
  SearchMovie,
} from './components';
import { listMoviesAdapter } from '../../adapters/listMoviesAdapter';
import { Wrapper } from '../../components';
import { useInputSearchMovie } from '../../store/inputSearchMovieStore';
import { useGetListMovies, useGetPopularMovies } from '../../hooks';

const MemoizedSearchMovie = memo(
  ({ value, onChange }: { value: string; onChange: (val: string) => void }) => {
    return <SearchMovie value={value} onChange={onChange} />;
  }
);

export const ListMoviePage = () => {
  const inputSearchMovie = useInputSearchMovie(state => state.inputSearchMovie);
  const {
    data,
    fetchNextPage,
    hasNextPage,
    isLoading,
    isFetchingNextPage,
    inputSearchDebounced,
    error,
  } = useGetListMovies();
  const { data: popularMovies, isLoading: isLoadingPopularMovies } = useGetPopularMovies();
  const setInputSearchMovie = useInputSearchMovie(state => state.setInputSearchMovie);

  const listMovies = useMemo(() => {
    const allMovies = data?.pages.flatMap(page => page.results) ?? [];
    if (!popularMovies) return [];
    return listMoviesAdapter(!inputSearchMovie ? popularMovies.results : allMovies);
  }, [data?.pages, popularMovies, inputSearchMovie]);

  return (
    <>
      <Wrapper>
        <MemoizedSearchMovie value={inputSearchMovie} onChange={setInputSearchMovie} />
      </Wrapper>
      <RenderLoader isLoading={isLoading} isLoadingPopularMovies={isLoadingPopularMovies} />
      <RenderMovieList
        isLoading={isLoading}
        isLoadingPopularMovies={isLoadingPopularMovies}
        fetchNextPage={fetchNextPage}
        hasNextPage={hasNextPage}
        isFetchingNextPage={isFetchingNextPage}
        listMovies={listMovies}
        inputSearchMovie={inputSearchMovie}
      />
      <RenderErrorLayout error={error} />
      {!error && (
        <RenderResultNotFoundLayout
          isLoading={isLoading}
          isLoadingPopularMovies={isLoadingPopularMovies}
          inputSearchDebounced={inputSearchDebounced}
          listMovies={listMovies}
        />
      )}
    </>
  );
};
