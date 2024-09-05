'use client';

import { useQuery } from '@tanstack/react-query';
import axios from 'axios';
import { usePathname } from 'next/navigation';

const fetchMovieDetails = async (id: string) => {
  const { data } = await axios.get(`https://api.themoviedb.org/3/movie/${id}`, {
    params: {
      api_key: process.env.NEXT_PUBLIC_TMDB_API_KEY,
    },
  });
  return data;
};

export default function MovieDetails() {
  const pathname = usePathname();
  const movieId = pathname.split('/').pop();

  const { data, isLoading, isError } = useQuery({
    queryKey: ['movie', movieId],
    queryFn: () => fetchMovieDetails(movieId as string),
    enabled: !!movieId,
  });

  if (isLoading) return <p>Loading...</p>;
  if (isError) return <p>Error loading movie details.</p>;

  return (
    <div className="movie-details">
      <h1>{data.title}</h1>
      <p>{data.overview}</p>
      <p>Release Date: {data.release_date}</p>
      <p>Rating: {data.vote_average} / 10</p>
    </div>
  );
}

