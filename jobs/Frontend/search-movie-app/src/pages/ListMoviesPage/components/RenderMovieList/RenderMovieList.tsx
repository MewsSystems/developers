import { useNavigate } from 'react-router';
import { Button, Spinner } from '../../../../components';
import type { RenderMovieListParams } from '../../../types';
import { CardMovie } from '../CardMovie';
import { WrapperListMovies } from '../WrapperListMovies';
import { SHOW_MORE_BUTTON_TEST_ID } from '../../../../constants';
import { useCallback } from 'react';
import { SHOW_MORE_BUTTON_LABEL } from '../../constants';

export const RenderMovieList: React.FC<RenderMovieListParams> = ({
  isLoading,
  isLoadingPopularMovies,
  listMovies,
  isFetchingNextPage,
  inputSearchMovie,
  hasNextPage,
  fetchNextPage,
}) => {
  const navigate = useNavigate();

  const handleOnClickCard = useCallback(
    (id: number): void => {
      navigate(`/details/${id}`);
    },
    [navigate]
  );

  const handleOnClickShowMoreButton = useCallback((): void => {
    fetchNextPage();
  }, [fetchNextPage]);

  if (!isLoading && !isLoadingPopularMovies && listMovies?.length) {
    return (
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
            data-testid={SHOW_MORE_BUTTON_TEST_ID}
          >
            {isFetchingNextPage ? <Spinner /> : SHOW_MORE_BUTTON_LABEL}
          </Button>
        ) : null}
      </>
    );
  }
  return null;
};
