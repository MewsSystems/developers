import { TvShowsWrapper } from "@/app/_components/tv-shows/wrapper";
import { getQueryClient } from "@/domain/queries/server-query-client";
import { tvShowsQuery } from "@/domain/queries/tv-shows-query";
import { TvType } from "@/domain/types/type";
import { HydrationBoundary, dehydrate } from "@tanstack/react-query";

export default async function TvShowsPage() {
  const { dehydratedState } = await useTvShowsPage();

  return (
    <HydrationBoundary state={dehydratedState}>
      <TvShowsWrapper />
    </HydrationBoundary>
  );
}

async function useTvShowsPage() {
  const queryClient = await getQueryClient();
  queryClient.prefetchQuery({
    queryKey: tvShowsQuery.key(TvType.TopRated),
    queryFn: () => tvShowsQuery.fnc(TvType.TopRated),
  });
  const dehydratedState = dehydrate(queryClient);

  return { dehydratedState };
}
