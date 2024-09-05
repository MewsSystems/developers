'use client'

import { useEffect, useState } from 'react';
import axios from 'axios';
import { useQuery } from '@tanstack/react-query';
import Link from 'next/link';
import Image from 'next/image';
import { useDebounce } from '@/hooks/useDebounce';

const fetchMovies = async (query: string, page: number = 1) => {
  const { data } = await axios.get(`https://api.themoviedb.org/3/search/movie`, {
    params: {
      api_key: process.env.NEXT_PUBLIC_TMDB_API_KEY,
      query: query,
      page: page,
    },
  });
  return data;
};


// example: http://localhost:3000/?query=lord%20of%20the&page=3
export default function SearchMovies() {
  const [query, setQuery] = useState('');
  const [page, setPage] = useState(1);
  const [totalPages, setTotalPages] = useState(1);
  const debouncedQuery = useDebounce(query, 500);

  const { data, isLoading, isError } = useQuery({
    queryKey: ['movies', debouncedQuery, page],
    queryFn: () => fetchMovies(debouncedQuery, page),
    enabled: !!debouncedQuery,
  });

  if (data?.total_pages > 1 && data?.total_pages !== totalPages) {
    setTotalPages(data?.total_pages)
  }

  const handleNextPage = () => {
    if (page < totalPages) {
      setPage(prevPage => prevPage + 1);
    }
  }

  const handlePrevPage = () => {
    if (page > 1) {
      setPage(prevPage => prevPage - 1);
    }
  };

  useEffect(() => {
    if (!debouncedQuery) {
      setPage(1);
      setTotalPages(1);
    }

  }, [debouncedQuery]);

  useEffect(() => {
    const searchParams = new URLSearchParams(window.location.search);
    const initialQuery = searchParams.get('query') || '';
    const initialPage = parseInt(searchParams.get('page') || '1', 10);
    console.log("query: ", initialQuery, "page: ", initialPage)

    setQuery(initialQuery);
    setPage(initialPage);
  }, []);

  return (
    <div className="container">
      <Image id="mewslifx-logo" src="/mewsflix.png" alt="Logo" width={200} height={50} className="logo" />

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
      {data?.results?.length === 0 && <p>There are no movies matching {query}</p>}

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

      <div className="pagination">
        <button onClick={handlePrevPage} disabled={page === 1}>
          {'<'}
        </button>
        <span>Page {page} of {totalPages}</span>
        <button onClick={handleNextPage} disabled={page === totalPages}>
          {'>'}
        </button>
      </div>
    </div>
  );
}
