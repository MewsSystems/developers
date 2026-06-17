import { useEffect } from 'react';
import { useLazyGetDiscoverMoviesQuery } from '../api/moviesApiSlice';
import useSearchMovies from './useSearchMovies';

const useDiscoverMovies = () => {
    const [getDiscoverMovies, result] = useLazyGetDiscoverMoviesQuery();
    const { data } = result;

    const { searchQuery } = useSearchMovies();

    useEffect(() => {
        if (searchQuery === '') {
            getDiscoverMovies(undefined, true);
        }
    }, []);

    return {
        getDiscoverMovies,
        discoverMovies: data,
    };
};

export default useDiscoverMovies;
