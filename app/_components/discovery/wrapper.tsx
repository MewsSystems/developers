"use client";

import { FC } from "react";
import { moviesQuery } from "@/domain/queries/movies-query";
import { MovieType, TvType } from "@/domain/types/type";
import { useQueries } from "@tanstack/react-query";
import { tvShowsQuery } from "@/domain/queries/tv-shows-query";
import { HeroSlider } from "./hero-slider";
import { ListContainer } from "./list-container";
import { Box } from "@/styles/base/box";
import { useTranslations } from "next-intl";

export const DiscoveryWrapper: FC = () => {
  const { t, popularMovies, upcomingMovies, popularTvShows, topRatedTvShows } =
    useDiscoveryWrapper();

  return (
    <Box $mb="xl">
      <HeroSlider movies={popularMovies?.slice(0, 5) ?? []} />
      <ListContainer title={t("trendingMovies")} items={popularMovies ?? []} />
      <ListContainer title={t("trendingTv")} items={popularTvShows ?? []} />
      <ListContainer title={t("topRatedTv")} items={topRatedTvShows ?? []} />
      <ListContainer title={t("upcomingMovies")} items={upcomingMovies ?? []} />
    </Box>
  );
};

function useDiscoveryWrapper() {
  const t = useTranslations("discovery");

  const [
    { data: popularMovies },
    { data: upcomingMovies },
    { data: popularTvShows },
    { data: topRatedTvShows },
  ] = useQueries({
    queries: [
      {
        queryKey: moviesQuery.key(MovieType.Popular),
        queryFn: () => moviesQuery.fnc(MovieType.Popular),
      },
      {
        queryKey: moviesQuery.key(MovieType.Upcoming),
        queryFn: () => moviesQuery.fnc(MovieType.Upcoming),
      },
      {
        queryKey: tvShowsQuery.key(TvType.Popular),
        queryFn: () => tvShowsQuery.fnc(TvType.Popular),
      },
      {
        queryKey: tvShowsQuery.key(TvType.TopRated),
        queryFn: () => tvShowsQuery.fnc(TvType.TopRated),
      },
    ],
  });

  return { t, popularMovies, upcomingMovies, popularTvShows, topRatedTvShows };
}
