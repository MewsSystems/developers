import { useState, useEffect } from 'react';
import axios from 'axios';
import { Movie, ApiResponse } from './types';

const API_KEY = '03b8572954325680265531140190fd2a';

const useMovieSearch = (query: string, page: number) => {
  const [movies, setMovies] = useState<Movie[]>([]);
  const [totalResults, setTotalResults] = useState<number>(0);
  const [hasMore, setHasMore] = useState<boolean>(true);
  const [cache, setCache] = useState<{
    [key: string]: { results: Movie[]; total_pages: number };
  }>({});

  useEffect(() => {
    const searchMovies = async (searchQuery: string, pageNum: number) => {
      if (searchQuery.length > 2) {
        const cacheKey = `${searchQuery}-${pageNum}`;
        if (cache[cacheKey]) {
          const cachedData = cache[cacheKey];
          if (pageNum === 1) {
            setMovies(cachedData.results);
          } else {
            setMovies((prevMovies) => [...prevMovies, ...cachedData.results]);
          }
          setHasMore(pageNum < cachedData.total_pages);
        } else {
          try {
            const response = await axios.get<ApiResponse>(
              'https://api.themoviedb.org/3/search/movie',
              {
                params: {
                  api_key: API_KEY,
                  query: searchQuery,
                  page: pageNum,
                },
              },
            );
            if (pageNum === 1) {
              setMovies(response.data.results);
            } else {
              setMovies((prevMovies) => [
                ...prevMovies,
                ...response.data.results,
              ]);
            }
            setTotalResults(response.data.total_results);
            setHasMore(pageNum < response.data.total_pages);
            setCache((prevCache) => ({
              ...prevCache,
              [cacheKey]: {
                results: response.data.results,
                total_pages: response.data.total_pages,
              },
            }));
          } catch (error) {
            console.error('Error fetching movies:', error);
          }
        }
      } else {
        setMovies([]);
        setTotalResults(0);
        setHasMore(false);
      }
    };

    if (query) {
      searchMovies(query, page);
    }
  }, [query, page, cache]);

  return { movies, totalResults, hasMore };
};

export default useMovieSearch;
