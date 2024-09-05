'use client';

import { useQuery } from '@tanstack/react-query';
import axios from 'axios';
import { usePathname } from 'next/navigation';
import Image from 'next/image';

const fetchMovieDetails = async (id: string) => {
  const { data } = await axios.get(`https://api.themoviedb.org/3/movie/${id}`, {
    params: {
      api_key: process.env.NEXT_PUBLIC_TMDB_API_KEY,
    },
  });
  return data;
};

// TODO add good details and have image on the left text on the right
// TODO add skeleton to image
export default function MovieDetails() {
  const pathname = usePathname();
  const movieId = pathname.split('/').pop();

  const { data, isLoading, isError } = useQuery({
    queryKey: ['movie', movieId],
    queryFn: () => fetchMovieDetails(movieId as string),
    enabled: !!movieId,
  });

  if (isLoading) return null
  if (isError) return <p>Error loading movie details.</p>;
  console.log("DATA: ", data)

  return (
    <div className="movie-details">
      <Image
        src={`https://image.tmdb.org/t/p/w200${data.poster_path}`}
        alt={data.title}
        width={200}
        height={400}
      />
      <div>
        <h1>{data.title}</h1>
        <p>{data.overview}</p>
        <p>Release Date: {data.release_date}</p>
        <p>Rating: {data.vote_average} / 10</p>
      </div>
    </div>
  );
}

