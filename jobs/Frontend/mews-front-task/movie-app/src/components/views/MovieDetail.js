import React, { useState, useEffect } from 'react';
import { useParams } from 'react-router-dom';
import MovieCard from './MovieCard';

function MovieDetail() {
  const { id } = useParams();
  const [movie, setMovie] = useState(null);

  useEffect(() => {
    const apiKey = '03b8572954325680265531140190fd2a';

    // Fetch details 
    fetch(`https://api.themoviedb.org/3/movie/${id}?api_key=${apiKey}`)
      .then(response => response.json())
      .then(data => setMovie(data))
      .catch(error => console.error('Error fetching movie details:', error));
  }, [id]);

  if (!movie) {
    return <div>Loading...</div>;
  }

  return (
    <div class="mt-5">
      <div className="row movie-section">
        <div className="col-md-2"></div>
          <div key={movie.id} className="col-md-3">
            <MovieCard
              movie={movie}
            />
          </div>
          <div className="col-md-5">
            <p>Release date: {movie.release_date}</p>
            <p>{movie.overview}</p>
            <p>Rating: {movie.vote_average} ({movie.vote_count} votes) </p>
            
          </div>
        <div className="col-md-2"></div>
      </div>
    </div>
  );
}

export default MovieDetail;
