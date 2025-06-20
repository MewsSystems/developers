import React from 'react';
import { Link } from 'react-router-dom';
import { Movie } from './types';

export default function MovieCard({ movie }: { movie: Movie }) {
  return (
    <Link to={`/movie/${movie.id}`} style={{ textDecoration: 'none', color: 'black', margin: 12 }}>
      <img
        src={`https://image.tmdb.org/t/p/w200${movie.poster_path}`}
        alt={movie.title}
        style={{ borderRadius: 8 }}
      />
      <h3>{movie.title}</h3>
    </Link>
  );
}