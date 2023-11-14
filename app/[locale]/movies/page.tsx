import { MoviesWrapper } from "@/app/_components/movies/wrapper";
import { moviesQuery } from "@/domain/queries/movies-query";
import { getQueryClient } from "@/domain/queries/server-query-client";
import { MovieType } from "@/domain/types/type";
import { HydrationBoundary, dehydrate } from "@tanstack/react-query";

export default async function MoviesPage() {
  const { dehydratedState } = await useMoviesPage();

  return (
    <HydrationBoundary state={dehydratedState}>
      <MoviesWrapper />
    </HydrationBoundary>
  );
}

async function useMoviesPage() {
  const queryClient = await getQueryClient();
  queryClient.prefetchQuery({
    queryKey: moviesQuery.key(MovieType.Popular),
    queryFn: () => moviesQuery.fnc(MovieType.TopRated),
  });
  const dehydratedState = dehydrate(queryClient);

  return { dehydratedState };
}
