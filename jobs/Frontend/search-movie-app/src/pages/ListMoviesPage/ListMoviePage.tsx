import { useNavigate } from 'react-router';
import { useGetListMovies } from '../../hooks';

const ListMoviePage = () => {
  const navigate = useNavigate();
  const { data: listMovies, isLoading } = useGetListMovies({ query: 'Fast', page: 2 });

  const handleOnClick = (id: number): void => {
    navigate(`details/${id}`);
  };

  return listMovies?.results.length && !isLoading ? (
    <>
      {listMovies?.results.map(movie => (
        <div onClick={() => handleOnClick(movie.id)} key={movie.id}>
          <p>Title: {movie.title}</p>
          <p>Overview: {movie.overview}</p>
          <p>Release date: {movie.release_date}</p>
        </div>
      ))}
    </>
  ) : null;
};

export { ListMoviePage };
