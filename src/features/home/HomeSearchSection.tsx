'use client';

import { useQuery } from '@tanstack/react-query';
import React, { useEffect, useRef } from 'react';
import { fetchMoviesClient } from '@/lib/fetch/fetchMoviesClient';
import { LoadingIndicator } from '@/components/LoadingIndicator';
import { MovieListItem, MovieListSkeleton } from '@/components/MovieListItem';
import { Pagination } from '@/components/Pagination';
import { ErrorMessage } from '@/components/ErrorMessage';
import { MovieSearchResponse } from '@/types/api';
import { DebouncedInput } from '@/components/DebouncedInput';
import { moviesQueryKey } from '@/lib/queryKeys';
import { useSsrHydratedUrlState } from '@/hooks/useSsrHydratedUrlState';
import { AccessibleResultsSummary } from '@/components/AccessibleResultsSummary';

const parsePageParam = (value: string | null): number => {
  let page = 1;
  if (value) {
    const n = Number(value);
    if (Number.isInteger(n) && n >= 1) {
      page = n;
    }
  }
  return page;
};

const parseSearchParam = (value: string | null): string => value ?? '';

const omitPageParam = (value: number) => value === 1;

interface Props {
  initialSearch: string;
  initialPage: number;
}

export function HomeSearchSection({ initialSearch, initialPage }: Props) {
  const { params, setParams } = useSsrHydratedUrlState({
    initialParams: { search: initialSearch, page: initialPage },
    paramParse: {
      page: parsePageParam,
      search: parseSearchParam,
    },
    shouldOmitParam: {
      page: omitPageParam,
    },
  });

  const queryKey = moviesQueryKey(params.search, params.page);
  const staleTime = Number(process.env.NEXT_PUBLIC_CLIENT_SIDE_SEARCH_REVALIDATE_TIME || 0) * 1000;
  const lastTotalPagesRef = useRef<number>(null);

  const resultsSummaryRef = useRef<HTMLDivElement>(null);

  const { data, isFetching, isError, isSuccess } = useQuery<MovieSearchResponse>({
    queryKey,
    queryFn: () => fetchMoviesClient(params.search, params.page),
    staleTime,
    enabled: !!params.search,
  });

  useEffect(() => {
    if (isSuccess && data) {
      lastTotalPagesRef.current = data.total_pages;
    }
  }, [isSuccess, data]);

  const movies = data?.results ?? [];
  const totalPages = data?.total_pages ?? lastTotalPagesRef.current ?? 0;
  const currentPage = params.page;
  const totalResults = data?.total_results ?? 0;

  const showPaging = !isError && params.search && totalPages > 1;

  const handleInputValue = (value: string) => setParams({ search: value, page: 1 });

  const handlePageChange = (page: number) => {
    setParams({ page });
    window.scrollTo({ top: 0 });
    setTimeout(() => {
      if (resultsSummaryRef.current) {
        resultsSummaryRef.current?.focus();
      }
    }, 250);
  };

  const title = params.search
    ? params.page && params.page > 1
      ? `Search: ${params.search} (Page ${params.page}) | Movie Search`
      : `Search: ${params.search} | Movie Search`
    : 'Movie Search';

  return (
    <section className="space-y-4">
      <title>{title}</title>
      <h1 className="text-xl font-extrabold text-stone-600 mb-1">Welcome to Movie Search</h1>
      <p className="text-stone-700">
        Use the search box to find your favorite movies. Results will appear below.
      </p>

      <div>
        <div className="relative flex items-center gap-2">
          <DebouncedInput
            value={params.search}
            onChange={handleInputValue}
            placeholder="Search movies..."
            className="border border-cyan-800 bg-white p-2 pr-6 flex-1 rounded"
            ariaLabel="Search movies"
          />
          {params.search && isFetching && <LoadingIndicator />}
        </div>

        <div className="min-h-[24px] mt-1">
          {isError && <ErrorMessage message="There was a problem fetching your search results" />}
          {!isError && !isFetching && params.search && (
            <AccessibleResultsSummary
              ref={resultsSummaryRef}
              currentPage={currentPage}
              totalPages={totalPages}
              totalItems={totalResults}
              pageSize={20}
              visible
            />
          )}
        </div>
      </div>

      <div className="min-h-8">
        {showPaging && (
          <Pagination
            data-testid="pagination-top"
            search={params.search}
            currentPage={currentPage}
            totalPages={totalPages}
            onPageChange={handlePageChange}
            readonly={isFetching}
            disableKeyboardNav
          />
        )}
      </div>

      <div className="flex flex-col gap-2" role="region" aria-label="search results">
        {isFetching ? (
          <MovieListSkeleton itemNumber={20} />
        ) : (
          movies.map((movie) => (
            <MovieListItem
              key={movie.id}
              movie={movie}
              search={params.search}
              page={currentPage > 1 ? currentPage : undefined}
            />
          ))
        )}
      </div>

      {showPaging && (
        <Pagination
          search={params.search}
          currentPage={currentPage}
          totalPages={totalPages}
          onPageChange={handlePageChange}
          readonly={isFetching}
        />
      )}
    </section>
  );
}
