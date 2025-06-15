import { useQuery } from '@tanstack/react-query';
import { fetchListMovies } from '../api/fetch';
import type { ListMoviesParams } from '../api/types';

const useGetListMovies = ({ query, page }: ListMoviesParams) => {
  return useQuery({
    queryKey: ['detailsMovie'],
    queryFn: () => fetchListMovies({ query, page }),
  });
};

export { useGetListMovies };
