'use client';

import { useQuery } from '@tanstack/react-query';
import axios from 'axios';
import { usePathname } from 'next/navigation';
import Image from 'next/image';
import { GenreTag } from '@/components/Tag/Tag';
import type { MovieDetails } from '@/models/movie';

// TODO move own file
const fetchMovieDetails = async (id: string): Promise<MovieDetails> => {
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

  if (isLoading) return null
  if (isError || !data) return <p>Error loading movie details.</p>;

  return (
    <div className="movie-details">
      <Image
        src={`https://image.tmdb.org/t/p/w200${data.poster_path}`}
        alt={data.title}
        width={200}
        height={400}
      />
      <section>
        <h1>{data.title}</h1>
        <p>{data.overview}</p>
        <p>Release on {data.release_date}</p>
        <h3>Rating: {data.vote_average.toFixed(1)}</h3>
        <div className="genres-container">
          {data.genres.map(genre => (
            <GenreTag key={genre.id} name={genre.name} />
          ))}
        </div>
      </section>
    </div>
  );
}

