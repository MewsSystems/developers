import { useState } from 'react';
import { API_ENDPOINTS } from '../endpoints';
import { Movie } from '../../components/MovieCard/types';
import { makeRequest } from '../makeRequest';

const useSearchMovies = () => {
  const [data, setData] = useState<Movie[]>([]);
  const [isLoading, setIsLoading] = useState<boolean>(false);
  const [error, setError] = useState<string>();
  const [totalPages, setTotalPages] = useState<number>(1);

  const search = (query_string: string, pageNumber = 1) => {
    setIsLoading(true);
    makeRequest(API_ENDPOINTS.searchByMovie, {
      query: query_string,
      page: pageNumber,
    })
      .then((res) => res.json())
      .then((data) => {
        // API Error
        if (data.status_message) {
          setError(data.status_message);
        } else {
          setData(data.results);
          setTotalPages(data.total_pages);
        }
      })
      .catch((error) => {
        setError(error);
      })
      .finally(() => setIsLoading(false));
  };

  return { data, isLoading, error, search, totalPages };
};

export default useSearchMovies;
