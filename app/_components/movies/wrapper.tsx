"use client";

import { FC, useState } from "react";
import { MovieType } from "@/domain/types/type";
import { Stack } from "@/styles/base/stack";
import { useInfiniteQuery } from "@tanstack/react-query";
import { GridList } from "@/styles/components/grid-list";
import { Movie } from "@/types/movie";
import { ListItem } from "../discovery/list-item";
import { Filters } from "./filters";
import { paginatedMoviesQuery } from "@/domain/queries/paginated-movies-query";
import { Button } from "@/styles/base/button";
import { Data } from "@/domain/remote/response/data";
import { flatMap } from "lodash";
import { useSearchParams } from "next/navigation";
import { Text } from "@/styles/base/text";
import { usePathname, useRouter } from "@/navigation";
import { useTranslations } from "next-intl";

export const MoviesWrapper: FC = () => {
  const { t, movies, isLoading, hasNextPage, handleLoadMore, handleMovieType } =
    useMoviesWrapper();

  const renderMovie = (movie: Movie) => (
    <ListItem key={movie.id} item={movie} />
  );

  return (
    <Stack $align="center" $gap="xl" $mt={90} $mb="xl">
      <Filters onCategoryClick={handleMovieType} />
      {!movies.length && !isLoading ? (
        <Text $fw={600} $mt="lg">
          {t("emptyPlaceholder")}
        </Text>
      ) : (
        <>
          <GridList>{movies?.map(renderMovie)}</GridList>
          {hasNextPage && (
            <Button onClick={handleLoadMore}>{t("loadMoreAction")}</Button>
          )}
        </>
      )}
    </Stack>
  );
};

function useMoviesWrapper() {
  const t = useTranslations("movies");
  const [type, setType] = useState<MovieType>(MovieType.TopRated);
  const searchParams = useSearchParams();
  const { search, year } = Object.fromEntries(searchParams.entries());
  const pathname = usePathname();
  const { replace } = useRouter();

  const { data, isLoading, hasNextPage, fetchNextPage } = useInfiniteQuery<
    Data<Movie>
  >({
    queryKey: paginatedMoviesQuery.key(type, {
      primary_release_year: Number(year),
      query: search,
    }),
    queryFn: ({ pageParam = 1 }) =>
      paginatedMoviesQuery.fnc(type, {
        page: pageParam as number,
        query: search,
        primary_release_year: year ? Number(year) : undefined,
      }),
    initialPageParam: 1,
    getNextPageParam: (lastPage) => {
      return lastPage.page < lastPage.total_pages
        ? lastPage.page + 1
        : lastPage.page;
    },
  });

  const handleLoadMore = async () => {
    await fetchNextPage();
  };

  const handleMovieType = (type: MovieType) => {
    replace(pathname);
    setType(type);
  };

  return {
    t,
    movies: flatMap(data?.pages, "results") ?? [],
    isLoading,
    hasNextPage,
    handleLoadMore,
    handleMovieType,
  };
}
