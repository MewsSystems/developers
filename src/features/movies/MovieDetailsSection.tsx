'use client';

import { useQuery } from '@tanstack/react-query';
import { fetchMovieDetailsClient } from '@/lib/fetchMovieDetailsClient';
import { movieDetailQueryKey } from '@/lib/queryKeys';
import { BackToSearchLink } from '@/components/BackToSearchLink';
import { MovieDetailResponse } from '@/types/api';
import { MovieDetailsView } from '@/components/MovieDetailsView';

type Props = {
  movieId: string;
};

export default function MovieDetailsSection({ movieId }: Props) {
  const queryKey = movieDetailQueryKey(movieId);
  const staleTime = Number(process.env.NEXT_PUBLIC_CLIENT_SIDE_MOVIE_REVALIDATE_TIME || 0) * 1000;

  const { data, isPending, isError } = useQuery<MovieDetailResponse>({
    queryKey,
    queryFn: () => fetchMovieDetailsClient(movieId),
    staleTime,
  });

  if (isPending) return <p>Loading...</p>;
  if (isError || !data) return <p>Error loading movie details.</p>;

  return (
    <section className="space-y-4">
      <BackToSearchLink />
      <MovieDetailsView movie={data} />
    </section>
  );
}
