import { Dispatch, SetStateAction, useEffect, useState } from 'react';
import { Movie, MovieApiResponse, sendRequest } from '../api/sendRequest';

export interface UseMovies {
  movies: Movie[];
  searchQuery: string;
  setSearchQuery: Dispatch<SetStateAction<string>>;
  page: number;
  incrementPageNumber: () => void;
  decrementPageNumber: () => void;
}

const useMovies = (): UseMovies => {
  const [movies, setMovies] = useState<Movie[]>([]);
  const [searchQuery, setSearchQuery] = useState<string>('');
  const [page, setPage] = useState<number>(1);
  const [numberOfPages, setNumberOfPages] = useState<number>(1);

  const incrementPageNumber = () => {
    setPage((previous) => {
      if (previous === numberOfPages) {
        return previous;
      }

      return previous + 1;
    });
  };

  const decrementPageNumber = () => {
    setPage((previous) => {
      if (previous === 1) {
        return previous;
      }

      return previous - 1;
    });
  };

  useEffect(() => {
    if (Boolean(searchQuery)) {
      sendRequest(searchQuery, page)
        .then((response: MovieApiResponse) => {
          setMovies(response.results);
          setNumberOfPages(response.total_pages);

          console.log('response: ', response);
        })
        .catch((error) => {
          console.error(error);
        });
    }
  }, [searchQuery, page]);

  return {
    movies,
    searchQuery,
    setSearchQuery,
    page,
    incrementPageNumber,
    decrementPageNumber,
  };
};

export { useMovies };
