import { memo } from 'react';
import { useNavigate } from 'react-router';
import { CardMovie, SearchMovie, CardSkeleton, WrapperListMovies } from './components';
import { listMoviesAdapter } from '../../adapters/listMoviesAdapter';
import { Button, Wrapper } from '../../components';
import { useInputSearchMovie } from '../../store/inputSearchMovieStore';
import { useGetListMovies } from '../../hooks/useGetListMovies';
import { Spinner } from '../../components/Spinner/Spinner';

const MemorizedSearchMovie = memo(
  ({ value, onChange }: { value: string; onChange: (val: string) => void }) => {
    return <SearchMovie value={value} onChange={onChange} />;
  }
);

const ListMoviePage = () => {
  const navigate = useNavigate();
  const inputSearchMovie = useInputSearchMovie(state => state.inputSearchMovie);
  const { data, fetchNextPage, hasNextPage, isLoading, isFetchingNextPage } = useGetListMovies({
    query: inputSearchMovie,
  });
  const setInputSearchMovie = useInputSearchMovie(state => state.setInputSearchMovie);
  const allMovies = data?.pages.flatMap(page => page.results) ?? [];
  const listMovies = listMoviesAdapter(allMovies);
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

      {!isLoading ? (
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
        <WrapperListMovies>
          {Array.from({ length: 8 }).map((_, i) => (
            <CardSkeleton key={i} data-testid="card-skeleton" />
          ))}
        </WrapperListMovies>
      )}
    </>
  );
};

export { ListMoviePage };
