import { DiscoveryWrapper } from "@/app/_components/discovery/wrapper";
import { moviesQuery } from "@/domain/queries/movies-query";
import { getQueryClient } from "@/domain/queries/server-query-client";
import { tvShowsQuery } from "@/domain/queries/tv-shows-query";
import { MovieType, TvType } from "@/domain/types/type";
import { HydrationBoundary, dehydrate } from "@tanstack/react-query";

export default async function DiscoveryPage() {
  const { dehydratedState } = await useDiscoveryPage();

  return (
    <HydrationBoundary state={dehydratedState}>
      <DiscoveryWrapper />
    </HydrationBoundary>
  );
}

async function useDiscoveryPage() {
  const queryClient = await getQueryClient();
  await Promise.all([
    queryClient.prefetchQuery({
      queryKey: moviesQuery.key(MovieType.Popular),
      queryFn: () => moviesQuery.fnc(MovieType.Popular),
    }),
    queryClient.prefetchQuery({
      queryKey: moviesQuery.key(MovieType.Upcoming),
      queryFn: () => moviesQuery.fnc(MovieType.Upcoming),
    }),
    queryClient.prefetchQuery({
      queryKey: tvShowsQuery.key(TvType.Popular),
      queryFn: () => tvShowsQuery.fnc(TvType.Popular),
    }),
    queryClient.prefetchQuery({
      queryKey: tvShowsQuery.key(TvType.TopRated),
      queryFn: () => tvShowsQuery.fnc(TvType.TopRated),
    }),
  ]);
  const dehydratedState = dehydrate(queryClient);

  return { dehydratedState };
}
