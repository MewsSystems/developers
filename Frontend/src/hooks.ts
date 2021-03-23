import { useCallback, useEffect, useRef, useState } from 'react';
import { TypedUseSelectorHook, useDispatch, useSelector } from 'react-redux';
import { NumberParam, StringParam, useQueryParams } from 'use-query-params';
import {
  configurationSelector,
  fetchConfigurationIfNeeded,
} from './redux/configurationReducer';
import { clear, fetchSearchResults } from './redux/searchReducer';
import { RootState, AppDispatch } from './store';

export const useAppDispatch = () => useDispatch<AppDispatch>();
export const useAppSelector: TypedUseSelectorHook<RootState> = useSelector;

// create poster urls
export const usePosterUrls = (path: string) => {
  const dispatch = useAppDispatch();
  const { images } = useAppSelector(configurationSelector);
  const { poster_sizes, secure_base_url } = images;

  useEffect(() => {
    dispatch(fetchConfigurationIfNeeded());
  }, [dispatch]);

  return useCallback(
    (width?: number) =>
      poster_sizes.map((size) => {
        const url = `${secure_base_url}${size}${path}`;

        if (!width || size === 'original') {
          return url;
        }

        const sizeNum = parseInt(size.replace(/\D/g, ''));
        const xDescriptor = Math.round((sizeNum / width) * 100) / 100;

        return `${url} ${xDescriptor}x`;
      }),
    [path, poster_sizes, secure_base_url]
  );
};

// fetch/clear search results
export const useMovieSearch = () => {
  const dispatch = useAppDispatch();
  const prevActionRef = useRef<any>(null);

  const searchMovies = useCallback(
    (query: string, page: number = 1) => {
      prevActionRef.current?.abort?.();

      if (query) {
        prevActionRef.current = dispatch(fetchSearchResults({ query, page }));
      } else {
        dispatch(clear());
        prevActionRef.current = null;
      }
    },
    [dispatch, prevActionRef]
  );

  return searchMovies;
};

export const useSearchQueryParams = () => {
  return useQueryParams({
    query: StringParam,
    page: NumberParam,
  });
};

// set page title
export const useDocumentTitle = (title: string) => {
  const [originalTitle] = useState(document.title);

  useEffect(() => {
    if (title) {
      document.title = title;
    }

    return () => {
      document.title = originalTitle;
    };
  }, [originalTitle, title]);
};
