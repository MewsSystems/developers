import { useState } from 'react';
import axios from 'axios';
import { Movie, ApiResponse } from './types';

const API_KEY = '03b8572954325680265531140190fd2a';

const fetchMovies = async (searchInput: string, page: number) => {
  try {
    const response = await axios.get<ApiResponse>(
      'https://api.themoviedb.org/3/search/movie',
      {
        params: {
          api_key: API_KEY,
          query: searchInput,
          page,
        },
      },
    );
    return response.data;
  } catch (error) {
    console.error('Error fetching movies:', error);
    return { results: [], total_results: 0, total_pages: 0 };
  }
};

const useMovieSearch = (initialSearchInput: string, initialPage: number) => {
  const [searchInput, setSearchInput] = useState<string>(initialSearchInput);
  const [page, setPage] = useState<number>(initialPage);
  const [movies, setMovies] = useState<Movie[]>([]);
  const [totalResults, setTotalResults] = useState<number>(0);
  const [hasMore, setHasMore] = useState<boolean>(true);

  const searchMovies = async (newSearchInput: string, newPage: number) => {
    if (newSearchInput.length > 2) {
      const data = await fetchMovies(newSearchInput, newPage);
      if (newPage === 1) {
        setMovies(data.results);
      } else {
        setMovies((prevMovies) => [...prevMovies, ...data.results]);
      }
      setTotalResults(data.total_results);
      setHasMore(newPage < data.total_pages);
    } else {
      setMovies([]);
      setTotalResults(0);
      setHasMore(false);
    }
  };

  return {
    searchInput,
    page,
    movies,
    totalResults,
    hasMore,
    setSearchInput,
    setPage,
    searchMovies,
  };
};

export default useMovieSearch;
