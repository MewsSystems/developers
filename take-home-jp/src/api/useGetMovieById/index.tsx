import { useState } from 'react';
import { API_ENDPOINTS } from '../endpoints';
import { Movie } from '../../components/MovieCard/types';
import { makeRequest } from '../makeRequest';

const useGetMovieById = () => {
  const [data, setData] = useState<Movie>();
  const [isLoading, setIsLoading] = useState<boolean>(false);
  const [error, setError] = useState<string>();

  const get = (movie_id: string) => {
    setIsLoading(true);
    makeRequest(API_ENDPOINTS.findById(movie_id))
      .then((res) => res.json())
      .then((data) => {
        setData(data);
      })
      .catch((error) => {
        setError(error);
      })
      .finally(() => setIsLoading(false));
  };

  return { data, isLoading, error, get };
};

export default useGetMovieById;
