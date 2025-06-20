import React, { useEffect, useState } from 'react';
import { useParams } from 'react-router-dom';
import { fetchMovie } from '../api';
import { MovieDetail } from '../types';

export default function MovieDetailPage() {
  const { id } = useParams();
  const [movie, setMovie] = useState<MovieDetail | null>(null);

  useEffect(() => {
    if (id) fetchMovie(id).then(setMovie);
  }, [id]);

  if (!movie) return <div style={{ padding: 20 }}>Loading...</div>;

  return (
    <div style={{ padding: 20 }}>
      <img src={`https://image.tmdb.org/t/p/w300${movie.poster_path}`} alt={movie.title} />
      <h1>{movie.title}</h1>
      <p><em>{movie.tagline}</em></p>
      <p><strong>Release:</strong> {movie.release_date}</p>
      <p><strong>Runtime:</strong> {movie.runtime} min</p>
      <p><strong>Genres:</strong> {movie.genres.map(g => g.name).join(', ')}</p>
      <p>{movie.overview}</p>
    </div>
  );
}