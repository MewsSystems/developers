'use client';

import { useQuery } from '@tanstack/react-query';
import { usePathname } from 'next/navigation';
import Image from 'next/image';
import { GenreTag } from '@/components/Tag/Tag';
import type { MovieDetails } from '@/models/movie';
import { fetchMovieDetails } from '@/api/movieApi';
import ErrorMessage from '@/components/ErrorMessage/ErrorMessage';

export default function MovieDetails() {
  const pathname = usePathname();
  const movieId = pathname.split('/').pop();

  const { data, isLoading, isError } = useQuery({
    queryKey: ['movie', movieId],
    queryFn: () => fetchMovieDetails(movieId as string),
    enabled: !!movieId,
  });

  if (isLoading) return null
  if (isError || !data) return <ErrorMessage>An error happened while loading this movie details</ErrorMessage>;

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
        <p>Released on {data.release_date}</p>
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

