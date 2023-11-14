import { MovieWrapper } from "@/app/_components/movie/wrapper";
import { movieQuery } from "@/domain/queries/movie-query";
import { getQueryClient } from "@/domain/queries/server-query-client";
import { HydrationBoundary, dehydrate } from "@tanstack/react-query";

type Params = { locale: string; id: string };
type Props = { params: Params };

export default async function MoviePage(props: Props) {
  const { dehydratedState } = await useMoviePage(props);

  return (
    <HydrationBoundary state={dehydratedState}>
      <MovieWrapper />
    </HydrationBoundary>
  );
}

async function useMoviePage({ params: { id } }: Props) {
  const queryClient = await getQueryClient();
  queryClient.prefetchQuery({
    queryKey: movieQuery.key(id),
    queryFn: () => movieQuery.fnc(id),
  });
  const dehydratedState = dehydrate(queryClient);

  return { dehydratedState };
}
