import { TvShowWrapper } from "@/app/_components/tv-show/wrapper";
import { getQueryClient } from "@/domain/queries/server-query-client";
import { tvShowQuery } from "@/domain/queries/tv-show-query";
import { HydrationBoundary, dehydrate } from "@tanstack/react-query";

type Params = { locale: string; id: string };
type Props = { params: Params };

export default async function TvShowPage(props: Props) {
  const { dehydratedState } = await useTvShowPage(props);

  return (
    <HydrationBoundary state={dehydratedState}>
      <TvShowWrapper />
    </HydrationBoundary>
  );
}

async function useTvShowPage({ params: { id } }: Props) {
  const queryClient = await getQueryClient();
  queryClient.prefetchQuery({
    queryKey: tvShowQuery.key(id),
    queryFn: () => tvShowQuery.fnc(id),
  });
  const dehydratedState = dehydrate(queryClient);

  return { dehydratedState };
}
