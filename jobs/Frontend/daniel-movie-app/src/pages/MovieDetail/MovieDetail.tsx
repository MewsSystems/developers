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
    // In case we haven't received the Movie object from Home, use id in url params to do API search
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
    <div className='flex flex-column align-items-center'>
      DETAILS {movie?.id} {movie?.title}
    </div>
  );
}

export default MovieDetail;
