import React from 'react';
import { Movie } from './types'; // Import Movie typu, který je definován ve společném souboru typů.

interface MovieListProps {
  movies: Movie[];
  onMovieClick: (id: number) => void;
}

const MovieList: React.FC<MovieListProps> = ({ movies, onMovieClick }) => {
  return (
    <ul>
      {movies.map((movie) => (
        <li key={movie.id} onClick={() => onMovieClick(movie.id)}>
          {movie.title}
          {movie.backdrop_path && (
            <img
              src={`https://image.tmdb.org/t/p/w500${movie.backdrop_path}`}
              alt={`${movie.title} backdrop`}
              loading="lazy"
            />
          )}
        </li>
      ))}
    </ul>
  );
};

export default MovieList;
