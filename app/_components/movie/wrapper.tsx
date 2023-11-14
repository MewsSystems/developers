"use client";

import { movieQuery } from "@/domain/queries/movie-query";
import { useQueries } from "@tanstack/react-query";
import { useParams } from "next/navigation";
import { FC } from "react";
import { ListContainer } from "../list/list-container";
import { useTranslations } from "next-intl";
import { ItemInfoModal } from "../store/item-info-modal";
import { useSelector } from "react-redux";
import { RootState } from "@/store";
import { Box } from "@/styles/base/box";
import { Details } from "../details/details";
import { LoadingContainer } from "@/styles/components/loading-container";
import { Text } from "@/styles/base/text";
import { actorsQuery } from "@/domain/queries/actors-query";
import { imagesQuery } from "@/domain/queries/images-query";
import { similarQuery } from "@/domain/queries/similar-query";
import { Movie } from "@/types/movie";
import { Title } from "@/styles/base/title";

export const MovieWrapper: FC = () => {
  const { t, movie, actors, images, similar, isModalOpen, isLoading } =
    useMovieWrapper();

  if (isLoading) {
    return (
      <LoadingContainer $justify="center" $align="center">
        <Title>{t("loading")}</Title>
      </LoadingContainer>
    );
  }

  return (
    <Box $my="xl">
      <Details
        item={movie as Movie}
        actors={actors ?? []}
        images={images ?? []}
      />
      <ListContainer title={t("similar")} items={(similar as Movie[]) ?? []} />
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
        queryKey: actorsQuery.key(id as string, "movie"),
        queryFn: () => actorsQuery.fnc(id as string, "movie"),
      },
      {
        queryKey: imagesQuery.key(id as string, "movie"),
        queryFn: () => imagesQuery.fnc(id as string, "movie"),
      },
      {
        queryKey: similarQuery.key(id as string, "movie"),
        queryFn: () => similarQuery.fnc(id as string, "movie"),
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
