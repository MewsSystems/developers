import { HydrationBoundary, dehydrate } from '@tanstack/react-query';
import { z } from 'zod';
import { getQueryClient } from '@/lib/getQueryClient';
import { fetchMovieDetails } from '@/lib/fetchMovieDetails';
import MovieDetailsSection from '@/features/movies/MovieDetailsSection';
import { movieDetailQueryKey } from '@/lib/queryKeys';

const movieIdSlugSchema = z.string().regex(/^(\d+)-.+$/, 'Invalid movie slug format');

type MoviePageProps = {
  params: Promise<{
    movieId: string;
  }>;
};

export default async function MoviePage({ params }: MoviePageProps) {
  const { movieId: movieSlug } = await params;

  const parsedSlug = movieIdSlugSchema.safeParse(movieSlug);
  if (!parsedSlug.success) {
    return <section className="text-red-600 p-4">Invalid movie slug format.</section>;
  }

  const match = parsedSlug.data.match(/^(\d+)-/);
  const movieId = match?.[1];

  if (!movieId) {
    return <section className="text-red-600 p-4">Could not extract a valid movie ID.</section>;
  }

  const queryClient = getQueryClient();

  await queryClient.prefetchQuery({
    queryKey: movieDetailQueryKey(movieId),
    queryFn: () => fetchMovieDetails(movieId),
  });

  return (
    <HydrationBoundary state={dehydrate(queryClient)}>
      <MovieDetailsSection movieId={movieId} />
    </HydrationBoundary>
  );
}
