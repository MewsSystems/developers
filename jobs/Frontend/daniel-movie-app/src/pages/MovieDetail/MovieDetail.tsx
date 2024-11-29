import { useEffect, useState } from 'react';
import './MovieDetail.css';
import { useParams } from 'react-router-dom';
import { useLocation } from 'react-router-dom';
import { fetchMovie } from '../../services/movieService';
import { Movie } from '../../models/Movie';

function MovieDetail() {
  const location = useLocation();
  const [movie, setMovie] = useState<Movie>(location.state?.movie || null);
  const { id } = useParams<{ id: string }>();

  useEffect(() => {
    // In case we haven't received the Movie object from MovieSearch, use id in url params to do API search
    if (!movie) {
      if (id) {
        console.log("Fetching movie details...")
        fetchMovie(id)
          .then((movie) => setMovie(movie))
          .catch((error) => console.error(error));
      } else {
        throw new Error("Couldn't retrieve movie id from params");
      }
    }
  }, []);

  return (
    <div className='flex flex m-4 sm:m-8 gap-5 flex-wrap'>
      <img src={'https://image.tmdb.org/t/p/w400/'+movie?.poster_path} alt={movie.title + " poster"} />
      <div className='flex flex-column' style={{ maxWidth: '60rem'}}>
        <h1>{movie?.title}</h1>
        <span className='font-italic'>Release date: {movie?.release_date}</span>
        <p className='mt-6'>{movie?.overview}</p>
       </div>
    </div>
  );
}

export default MovieDetail;
