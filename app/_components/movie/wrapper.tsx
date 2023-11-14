"use client";

import { movieActorsQuery } from "@/domain/queries/movie-actors-query";
import { movieImagesQuery } from "@/domain/queries/movie-images-query";
import { movieQuery } from "@/domain/queries/movie-query";
import { movieSimilarQuery } from "@/domain/queries/movie-similar-query";
import { useQueries } from "@tanstack/react-query";
import { useParams } from "next/navigation";
import { FC } from "react";
import { ListContainer } from "../list/list-container";
import { useTranslations } from "next-intl";
import { ItemInfoModal } from "../store/item-info-modal";
import { useSelector } from "react-redux";
import { RootState } from "@/store";
import { Box } from "@/styles/base/box";
import { Details } from "./details";
import { LoadingContainer } from "@/styles/components/loading-container";
import { Text } from "@/styles/base/text";

export const MovieWrapper: FC = () => {
  const { t, movie, actors, images, similar, isModalOpen, isLoading } =
    useMovieWrapper();

  if (isLoading) {
    return (
      <LoadingContainer $justify="center" $align="center">
        <Text>{t("loading")}</Text>
      </LoadingContainer>
    );
  }

  return (
    <Box $my="xl">
      <Details movie={movie!} actors={actors ?? []} images={images ?? []} />
      <ListContainer title={t("similar")} items={similar ?? []} />
      {isModalOpen && <ItemInfoModal />}
    </Box>
  );
};

function useMovieWrapper() {
  const t = useTranslations("movie");
  const { id } = useParams();
  const { isOpen: isModalOpen } = useSelector(
    (state: RootState) => state.modal
  );

  const [
    { data: movie, isLoading: isMovieLoading },
    { data: actors, isLoading: isActorsLoading },
    { data: images, isLoading: isImagesLoading },
    { data: similar, isLoading: isSimilarLoading },
  ] = useQueries({
    queries: [
      {
        queryKey: movieQuery.key(id as string),
        queryFn: () => movieQuery.fnc(id as string),
      },
      {
        queryKey: movieActorsQuery.key(id as string),
        queryFn: () => movieActorsQuery.fnc(id as string),
      },
      {
        queryKey: movieImagesQuery.key(id as string),
        queryFn: () => movieImagesQuery.fnc(id as string),
      },
      {
        queryKey: movieSimilarQuery.key(id as string),
        queryFn: () => movieSimilarQuery.fnc(id as string),
      },
    ],
  });

  return {
    t,
    movie,
    actors,
    images,
    similar,
    isModalOpen,
    isLoading:
      isMovieLoading || isActorsLoading || isImagesLoading || isSimilarLoading,
  };
}
