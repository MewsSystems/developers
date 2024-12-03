import './MovieDetail.css';
import { useEffect, useState } from 'react';
import { useNavigate, useParams } from 'react-router';
import { fetchMovie } from '../../services/movieService';
import { Movie } from '../../models/Movie';
import { Button } from 'primereact/button';

function MovieDetail() {
  const navigate = useNavigate();
  const [movie, setMovie] = useState<Movie>();
  const { id } = useParams<{ id: string }>();

  useEffect(() => {
    fetchMovie(id)
      .then((movie) => setMovie(movie))
      .catch((error) => console.error(error));
  }, [id]);

  return (
    <div className='flex flex-column m-2 sm:m-5 gap-4 sm:gap-6'>
      <Button 
        icon="pi pi-chevron-left" 
        rounded 
        text 
        onClick={() => navigate('/')}
      ></Button>
      <div className='flex flex-wrap gap-5 mx-3 sm:mx-6'>
        {movie ? (
          <>
            <img src={'https://image.tmdb.org/t/p/w300/'+movie.poster_path} alt={movie.title + " poster"} />
            <div className='flex flex-column' style={{ maxWidth: '60rem'}}>
              <h1>{movie.title}</h1>
              <span className='font-italic'>Release date: {movie.release_date ?? "-"}</span>
              <p className='mt-6'>{movie.overview}</p>
            </div>
          </>
         ) : (
          <div>No movie found</div>
        )}
      </div>
    </div>
  );
}

export default MovieDetail;
