import React from 'react';
import { useParams } from 'react-router-dom';
import MovieDetails from './movieComponent/MovieDetails';

const MovieDetailsWrapper = () => {
    const { id } = useParams<any>();
  
    if (!id) {
      // Handle case where id is undefined
      return <div>Invalid URL</div>;
    }
  
    const movieId = parseInt(id, 10);
  
    if (isNaN(movieId)) {
      // Handle case where id is not a valid number
      return <div>Invalid Movie ID</div>;
    }
  
    return <MovieDetails movieId={movieId} />;
  };


export default MovieDetailsWrapper;
