"use client";

import { useQueries } from "@tanstack/react-query";
import { useParams } from "next/navigation";
import { FC } from "react";
import { ListContainer } from "../list/list-container";
import { useTranslations } from "next-intl";
import { ItemInfoModal } from "../store/item-info-modal";
import { useSelector } from "react-redux";
import { RootState } from "@/store";
import { Box } from "@/styles/base/box";
import { LoadingContainer } from "@/styles/components/loading-container";
import { Text } from "@/styles/base/text";
import { actorsQuery } from "@/domain/queries/actors-query";
import { imagesQuery } from "@/domain/queries/images-query";
import { similarQuery } from "@/domain/queries/similar-query";
import { tvShowQuery } from "@/domain/queries/tv-show-query";
import { Details } from "../details/details";
import { TvShow } from "@/types/tv-show";
import { Title } from "@/styles/base/title";

export const TvShowWrapper: FC = () => {
  const { t, tv, actors, images, similar, isModalOpen, isLoading } =
    useTvShowWrapper();

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
        item={tv as TvShow}
        actors={actors ?? []}
        images={images ?? []}
      />
      <ListContainer title={t("similar")} items={(similar as TvShow[]) ?? []} />
      {isModalOpen && <ItemInfoModal />}
    </Box>
  );
};

function useTvShowWrapper() {
  const t = useTranslations("tv");
  const { id } = useParams();
  const { isOpen: isModalOpen } = useSelector(
    (state: RootState) => state.modal
  );

  const [
    { data: tv, isLoading: isTvLoading },
    { data: actors, isLoading: isActorsLoading },
    { data: images, isLoading: isImagesLoading },
    { data: similar, isLoading: isSimilarLoading },
  ] = useQueries({
    queries: [
      {
        queryKey: tvShowQuery.key(id as string),
        queryFn: () => tvShowQuery.fnc(id as string),
      },
      {
        queryKey: actorsQuery.key(id as string, "tv"),
        queryFn: () => actorsQuery.fnc(id as string, "tv"),
      },
      {
        queryKey: imagesQuery.key(id as string, "tv"),
        queryFn: () => imagesQuery.fnc(id as string, "tv"),
      },
      {
        queryKey: similarQuery.key(id as string, "tv"),
        queryFn: () => similarQuery.fnc(id as string, "tv"),
      },
    ],
  });

  return {
    t,
    tv,
    actors,
    images,
    similar,
    isModalOpen,
    isLoading:
      isTvLoading || isActorsLoading || isImagesLoading || isSimilarLoading,
  };
}
