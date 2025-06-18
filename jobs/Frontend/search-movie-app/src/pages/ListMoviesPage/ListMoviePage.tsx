import { memo } from 'react';
import { useNavigate } from 'react-router';
import {
  CardMovie,
  SearchMovie,
  CardSkeleton,
  WrapperListMovies,
  NoMoviesFound,
} from './components';
import { listMoviesAdapter } from '../../adapters/listMoviesAdapter';
import { Button, Wrapper, Spinner } from '../../components';
import { useInputSearchMovie } from '../../store/inputSearchMovieStore';
import { useGetListMovies, useGetPopularMovies } from '../../hooks';

const MemorizedSearchMovie = memo(
  ({ value, onChange }: { value: string; onChange: (val: string) => void }) => {
    return <SearchMovie value={value} onChange={onChange} />;
  }
);

export const ListMoviePage = () => {
  const navigate = useNavigate();
  const inputSearchMovie = useInputSearchMovie(state => state.inputSearchMovie);
  const { data, fetchNextPage, hasNextPage, isLoading, isFetchingNextPage, inputSearchDebounced } =
    useGetListMovies();
  const { data: popularMovies, isLoading: isLoadingPopularMovies } = useGetPopularMovies();
  const setInputSearchMovie = useInputSearchMovie(state => state.setInputSearchMovie);
  const allMovies = data?.pages.flatMap(page => page.results) ?? [];
  const listMovies =
    popularMovies && listMoviesAdapter(!inputSearchMovie ? popularMovies?.results : allMovies);
  const handleOnClickCard = (id: number): void => {
    navigate(`/details/${id}`);
  };

  const handleOnClickShowMoreButton = (): void => {
    fetchNextPage();
  };

  return (
    <>
      <Wrapper>
        <MemorizedSearchMovie value={inputSearchMovie} onChange={setInputSearchMovie} />
      </Wrapper>
      {isLoading || isLoadingPopularMovies ? (
        <WrapperListMovies>
          {Array.from({ length: 8 }).map((_, i) => (
            <CardSkeleton key={i} data-testid="card-skeleton" />
          ))}
        </WrapperListMovies>
      ) : listMovies?.length ? (
        <>
          <WrapperListMovies>
            {listMovies.map(movie => (
              <CardMovie
                data={movie}
                handleOnClick={() => handleOnClickCard(movie.id)}
                key={movie.id}
              ></CardMovie>
            ))}
          </WrapperListMovies>
          {inputSearchMovie && hasNextPage ? (
            <Button
              onClick={handleOnClickShowMoreButton}
              disabled={isFetchingNextPage || isLoading}
              data-testid="show-more-button"
            >
              {isFetchingNextPage ? <Spinner /> : 'Show more'}
            </Button>
          ) : null}
        </>
      ) : (
        inputSearchDebounced && <NoMoviesFound />
      )}
    </>
  );
};
