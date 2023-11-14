"use client";

import { FC, useState } from "react";
import { MovieType, TvType } from "@/domain/types/type";
import { Stack } from "@/styles/base/stack";
import { useInfiniteQuery } from "@tanstack/react-query";
import { GridList } from "@/styles/components/grid-list";
import { ListItem } from "../discovery/list-item";
import { Button } from "@/styles/base/button";
import { Data } from "@/domain/remote/response/data";
import { flatMap } from "lodash";
import { useSearchParams } from "next/navigation";
import { Text } from "@/styles/base/text";
import { usePathname, useRouter } from "@/navigation";
import { useTranslations } from "next-intl";
import { TvShow } from "@/types/tv-show";
import { paginatedTvShowsQuery } from "@/domain/queries/paginated-tv-shows-query";
import { Filters } from "../filters";

export const TvShowsWrapper: FC = () => {
  const { t, tvShows, isLoading, hasNextPage, handleLoadMore, handleTvType } =
    useTvShowsWrapper();

  const renderTv = (tvShow: TvShow) => (
    <ListItem key={tvShow.id} item={tvShow} />
  );

  return (
    <Stack $align="center" $gap="xl" $mt={90} $mb="xl">
      <Filters onCategoryClick={handleTvType} />
      {!tvShows.length && !isLoading ? (
        <Text $fw={600} $mt="lg">
          {t("emptyPlaceholder")}
        </Text>
      ) : (
        <>
          <GridList>{tvShows?.map(renderTv)}</GridList>
          {hasNextPage && (
            <Button onClick={handleLoadMore}>{t("loadMoreAction")}</Button>
          )}
        </>
      )}
    </Stack>
  );
};

function useTvShowsWrapper() {
  const t = useTranslations("tvShows");
  const [type, setType] = useState<TvType>(TvType.Popular);
  const searchParams = useSearchParams();
  const { search, year } = Object.fromEntries(searchParams.entries());
  const pathname = usePathname();
  const { replace } = useRouter();

  const { data, isLoading, hasNextPage, fetchNextPage } = useInfiniteQuery<
    Data<TvShow>
  >({
    queryKey: paginatedTvShowsQuery.key(type, {
      primary_release_year: Number(year),
      query: search,
    }),
    queryFn: ({ pageParam = 1 }) =>
      paginatedTvShowsQuery.fnc(type, {
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

  // TODO: fix types
  const handleTvType = (type: MovieType) => {
    replace(pathname);
    setType(type as any);
  };

  return {
    t,
    tvShows: flatMap(data?.pages, "results") ?? [],
    isLoading,
    hasNextPage,
    handleLoadMore,
    handleTvType,
  };
}
