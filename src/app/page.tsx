import { HydrationBoundary, dehydrate } from '@tanstack/react-query';
import { fetchMovies } from '@/lib/fetch/fetchMovies';
import { HomeSearchSection } from '@/features/home/HomeSearchSection';
import { getQueryClient } from '@/lib/getQueryClient';
import { moviesQueryKey } from '@/lib/queryKeys';

export const revalidate = 300;

interface HomePageProps {
  searchParams: Promise<Record<string, string | undefined>>;
}

export default async function HomePage({ searchParams }: HomePageProps) {
  const { search = '', page = '1' } = await searchParams;
  const parsedPage = parseInt(page, 10);

  const queryClient = getQueryClient();

  if (search) {
    await queryClient.prefetchQuery({
      queryKey: moviesQueryKey(search, parsedPage),
      queryFn: () => fetchMovies(search, parsedPage),
    });
  }

  return (
    <HydrationBoundary state={dehydrate(queryClient)}>
      <HomeSearchSection initialSearch={search} initialPage={parsedPage} />
    </HydrationBoundary>
  );
}
