'use client';

import { useQuery } from '@tanstack/react-query';
import React, { useEffect, useRef, useState } from 'react';
import { useSearchParams, useRouter, usePathname } from 'next/navigation';

import { fetchMoviesClient } from '@/lib/fetchMoviesClient';
import { LoadingIndicator } from '@/components/LoadingIndicator';
import { MovieListItem } from '@/components/MovieListItem';
import { Pagination } from '@/components/Pagination';
import { ErrorMessage } from '@/components/ErrorMessage';
import { MovieSearchResponse } from '@/types/api';
import { DebouncedInput } from '@/components/DebouncedInput';
import { moviesQueryKey } from '@/lib/queryKeys';
import { MovieListSkeleton } from '@/components/MovieListSkeleton';

export function HomeSearchSection() {
  const { replace } = useRouter();
  const searchParams = useSearchParams();
  const pathname = usePathname();

  const searchParam = searchParams.get('search') ?? '';
  const pageParam = parseInt(searchParams.get('page') ?? '1', 10);

  const [search, setSearch] = useState(searchParam);
  const [page, setPage] = useState(pageParam);

  const queryKey = moviesQueryKey(search, page);
  const staleTime = Number(process.env.NEXT_PUBLIC_CLIENT_SIDE_SEARCH_REVALIDATE_TIME || 0) * 1000;

  const lastDataRef = useRef<MovieSearchResponse | null>(null);

  const { data, isPending, isFetching, isLoading, isError, isSuccess } =
    useQuery<MovieSearchResponse>({
      queryKey,
      queryFn: () => fetchMoviesClient(search, page),
      staleTime,
      enabled: !!search,
    });

  useEffect(() => {
    if (isSuccess && data) {
      lastDataRef.current = data;
    }
  }, [isSuccess, data]);

  useEffect(() => {
    const params = new URLSearchParams(searchParams);
    if (search) {
      params.set('search', search);
    } else {
      params.delete('search');
    }

    if (search && Number.isFinite(page) && page > 1) {
      params.set('page', page.toString());
    } else {
      params.delete('page');
    }
    replace(`${pathname}?${params.toString()}`);
  }, [search, page, pathname, searchParams, replace]);

  const handlePageChange = (newPage: number) => {
    setPage(newPage);
  };

  const handleDebouncedInputChange = (value: string) => {
    setSearch(value);
    setPage(1);
  };

  const movies = data?.results ?? lastDataRef.current?.results ?? [];
  const totalPages = data?.total_pages ?? lastDataRef.current?.total_pages ?? 0;
  const currentPage = page;

  return (
    <section className="space-y-4">
      <h1 className="text-xl font-bold text-stone-800">Welcome to Movie Search</h1>
      <p className="text-stone-600">
        Use the search box to find your favorite movies. Results will appear below.
      </p>

      <div>
        <div className="relative flex items-center gap-2">
          <DebouncedInput
            value={searchParam}
            onChange={handleDebouncedInputChange}
            placeholder="Search movies..."
            className="border border-purple-800 p-2 pr-6 flex-1 rounded"
            ariaLabel="Search movies"
          />
          {search && (isPending || isFetching) && <LoadingIndicator />}
        </div>

        <div className="min-h-[24px] mt-1" aria-live="polite" aria-atomic="true">
          {isError ? (
            <ErrorMessage message="There was a problem fetching your search results" />
          ) : search && !isFetching && movies.length === 0 ? (
            <p className="text-stone-800">No results match your search</p>
          ) : null}
        </div>
      </div>

      {totalPages > 1 && (
        <Pagination
          data-testid="pagination-top"
          currentPage={currentPage}
          totalPages={totalPages}
          search={search}
          onPageChange={handlePageChange}
        />
      )}

      <div className="flex flex-col gap-2" role="region" aria-live="polite" aria-atomic="true">
        {isLoading && !lastDataRef.current ? (
          <MovieListSkeleton itemNumber={20} />
        ) : (
          movies.map((movie) => (
            <MovieListItem
              key={movie.id}
              movie={movie}
              search={search}
              page={page > 1 ? page : undefined}
            />
          ))
        )}
      </div>

      {totalPages > 1 && (
        <Pagination
          currentPage={currentPage}
          totalPages={totalPages}
          search={search}
          onPageChange={handlePageChange}
        />
      )}
    </section>
  );
}
