import React from 'react';
import './MovieCard.css';

const MovieCard = ({ movie, onClick }) => {
  return (
    <div className="movie-card d-flex flex-column align-items-center" onClick={onClick}>
      <img
        src={`https://image.tmdb.org/t/p/w500${movie.poster_path}`}
        alt={`${movie.title} Poster`}
        className="movie-poster"
      />
      <p className="movie-title">{movie.title}</p>
    </div>
  );
};

export default MovieCard;
