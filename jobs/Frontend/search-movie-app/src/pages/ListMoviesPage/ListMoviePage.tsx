import { useNavigate } from 'react-router';
import { useGetListMovies } from '../../hooks';
import { CardMovie, SearchMovie, CardSkeleton, WrapperListMovies } from './components';
import { listMoviesAdapter } from '../../adapters/listMoviesAdapter';
import { Button, Wrapper } from '../../components';
import { useInputSearchMovie } from '../../store/inputSearchMovieStore';

const ListMoviePage = () => {
  const navigate = useNavigate();
  const inputSearchMovie = useInputSearchMovie(state => state.inputSearchMovie);
  const { data, isLoading } = useGetListMovies({ query: inputSearchMovie, page: 2 });
  const setInputSearchMovie = useInputSearchMovie(state => state.setInputSearchMovie);
  const listMovies = data && listMoviesAdapter(data);

  const handleOnClickCard = (id: number): void => {
    navigate(`details/${id}`);
  };

  const handleOnClickShowMoreButton = (): void => {
    console.log('handleOnClickShowMoreButton');
  };

  return listMovies?.listMovies.length && !isLoading ? (
    <>
      <Wrapper>
        <SearchMovie value={inputSearchMovie} onChange={setInputSearchMovie} />
      </Wrapper>
      <WrapperListMovies>
        {isLoading
          ? Array.from({ length: 20 }).map((_, i) => <CardSkeleton key={i} />)
          : listMovies?.listMovies.map(movie => (
              <CardMovie
                data={movie}
                handleOnClick={() => handleOnClickCard(movie.id)}
                key={movie.id}
              ></CardMovie>
            ))}
      </WrapperListMovies>
      <Button onClick={handleOnClickShowMoreButton}>Show more</Button>
    </>
  ) : null;
};

export { ListMoviePage };
