'use client'

import { useState } from 'react';
import axios from 'axios';
import { useQuery } from '@tanstack/react-query';
import Link from 'next/link';
import Image from 'next/image';
import { useDebounce } from '../hooks/useDebounce';

const fetchMovies = async (query: string) => {
  const { data } = await axios.get(`https://api.themoviedb.org/3/search/movie`, {
    params: {
      api_key: process.env.NEXT_PUBLIC_TMDB_API_KEY,
      query: query,
    },
  });
  return data;
};

export default function SearchMovies() {
  const [query, setQuery] = useState('');
  const debouncedQuery = useDebounce(query, 500);

  const { data, isLoading, isError } = useQuery({
    queryKey: ['movies', debouncedQuery],
    queryFn: () => fetchMovies(debouncedQuery),
    enabled: !!debouncedQuery,
  });

  return (
    <div className="container">
      <input
        type="text"
        placeholder="Search for a movie..."
        value={query}
        onChange={(e) => setQuery(e.target.value)}
        className="search-input"
        autoFocus
      />

      {isLoading && <p>Loading...</p>}
      {isError && <p>Error fetching movies.</p>}

      <div className="movie-list">
        {data?.results?.map(movie => (
          <Link key={movie.id} href={`/movie/${movie.id}`}>
            <h3>{movie.title}</h3>
            <Image
              src={`https://image.tmdb.org/t/p/w200${movie.poster_path}`}
              alt={movie.title}
              width={200}
              height={300}
            />
          </Link>
        ))}
      </div>
    </div>
  );
}
