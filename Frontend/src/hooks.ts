import { useCallback, useEffect, useRef } from 'react';
import { TypedUseSelectorHook, useDispatch, useSelector } from 'react-redux';
import {
  configurationSelector,
  fetchConfigurationIfNeeded,
} from './redux/configurationReducer';
import { clear, fetchSearchResults } from './redux/searchReducer';
import { RootState, AppDispatch } from './store';

export const useAppDispatch = () => useDispatch<AppDispatch>();
export const useAppSelector: TypedUseSelectorHook<RootState> = useSelector;

export const usePosterUrls = (path: string, width: number) => {
  const dispatch = useAppDispatch();
  const { images } = useAppSelector(configurationSelector);
  const { poster_sizes, secure_base_url } = images;

  useEffect(() => {
    dispatch(fetchConfigurationIfNeeded());
  }, [dispatch]);

  return useCallback(
    (withX: boolean = false) =>
      poster_sizes.map((size) => {
        const url = `${secure_base_url}${size}${path}`;

        if (!withX || size === 'original') {
          return url;
        }

        const sizeNum = parseInt(size.replace(/\D/g, ''));
        const xDescriptor = Math.round((sizeNum / width) * 100) / 100;

        return `${url} ${xDescriptor}x`;
      }),
    [path, poster_sizes, secure_base_url, width]
  );
};

export const useMovieSearch = () => {
  const dispatch = useAppDispatch();
  const lastActionRef = useRef<any>(null);
  const { query, page, isLoading, results, error } = useAppSelector(
    (state) => state.search
  );

  const searchMovies = useCallback(
    (query: string, page: number = 1) => {
      if (isLoading && typeof lastActionRef.current.abort === 'function') {
        lastActionRef.current.abort();
      }

      if (query) {
        lastActionRef.current = dispatch(fetchSearchResults({ query, page }));
      } else {
        dispatch(clear());
        lastActionRef.current = null;
      }
    },
    [dispatch, isLoading]
  );

  return {
    query,
    page,
    isLoading,
    results,
    error,
    searchMovies,
  };
};
