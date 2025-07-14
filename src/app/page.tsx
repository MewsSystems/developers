import { HydrationBoundary, dehydrate } from '@tanstack/react-query';
import { fetchMovies } from '@/lib/fetch/app-serverside/fetchMovies';
import { HomeSearchSection } from '@/features/home/HomeSearchSection';
import { getQueryClient } from '@/lib/getQueryClient';
import { moviesQueryKey } from '@/lib/queryKeys';
import { movieSearchQuerySchema } from '@/lib/validation/movieSearchQuerySchema';

export const revalidate = 300;

interface HomePageProps {
  searchParams: Promise<Record<string, string | undefined>>;
}

export default async function HomePage({ searchParams }: HomePageProps) {
  const rawParams = await searchParams;

  const searchSchema = movieSearchQuerySchema.shape.search;
  const pageSchema = movieSearchQuerySchema.shape.page;

  const searchResult = searchSchema.safeParse(rawParams.search);
  const pageResult = pageSchema.safeParse(rawParams.page);

  const search = searchResult.success ? searchResult.data : '';
  const page = pageResult.success ? pageResult.data : 1;

  const queryClient = getQueryClient();

  if (search) {
    await queryClient.prefetchQuery({
      queryKey: moviesQueryKey(search, page),
      queryFn: () => fetchMovies(search, page),
    });
  }

  return (
    <HydrationBoundary state={dehydrate(queryClient)}>
      <HomeSearchSection initialSearch={search} initialPage={page} />
    </HydrationBoundary>
  );
}
