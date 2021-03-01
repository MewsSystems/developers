import { useCallback, useRef } from 'react';
import { TypedUseSelectorHook, useDispatch, useSelector } from 'react-redux';
import { clear, fetchSearchResults } from './redux/searchReducer';
import { RootState, AppDispatch } from './store';

export const useAppDispatch = () => useDispatch<AppDispatch>();
export const useAppSelector: TypedUseSelectorHook<RootState> = useSelector;

export const useImageConfig = () => {
  const { images } = useAppSelector((state) => state.configuration);

  return {
    ...images,
    getURLs: (path: string, sizes: string[]) =>
      path && sizes
        ? sizes.map((size) => `${images.secure_base_url}${size}${path}`)
        : [],
  };
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
