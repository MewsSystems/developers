import { apiConfig } from "@/domain/remote/config";
import { useRouter } from "@/navigation";
import { paths } from "@/navigation/paths";
import { RootState } from "@/store";
import { closeModal } from "@/store/slices/item-modal";
import { PrimaryButton } from "@/styles/base/button";
import { Group } from "@/styles/base/group";
import { Title } from "@/styles/base/title";
import {
  InfoModal,
  InfoModalCloseBlock,
  InfoModalContentImageBlock,
  InfoModalContentInfo,
  InfoModalContentInfoText,
  InfoModalImage,
} from "@/styles/components/info-modal";
import { IconX } from "@tabler/icons-react";
import { useTranslations } from "next-intl";
import Image from "next/image";
import { FC } from "react";
import { useDispatch, useSelector } from "react-redux";

export const ItemInfoModal: FC = () => {
  const {
    t,
    title,
    overview,
    coverImage,
    posterImage,
    handleClose,
    handleNavigation,
  } = useInfoModal();

  return (
    <InfoModal>
      <Group $p="md" $justify="space-around" $align="stretch" $gap="md">
        <InfoModalContentImageBlock>
          <Image src={posterImage} fill alt={title} />
        </InfoModalContentImageBlock>
        <InfoModalContentInfo>
          <Title>{title}</Title>
          <InfoModalContentInfoText>{overview}</InfoModalContentInfoText>
          <PrimaryButton onClick={handleNavigation}>
            {t("watchAction")}
          </PrimaryButton>
        </InfoModalContentInfo>
      </Group>
      <InfoModalImage $bgImage={coverImage}></InfoModalImage>
      <InfoModalCloseBlock onClick={handleClose}>
        <IconX />
      </InfoModalCloseBlock>
    </InfoModal>
  );
};

function useInfoModal() {
  const t = useTranslations("shared.infoModal");
  const { item } = useSelector((state: RootState) => state.modal);
  const dispatch = useDispatch();
  const { push } = useRouter();

  const title = item?.title ?? item?.name ?? "";
  const overview = item?.overview ?? null;

  const coverImage = apiConfig.coverImage(
    item?.backdrop_path || item?.poster_path || ""
  );
  const posterImage = apiConfig.posterImage(
    item?.poster_path || item?.backdrop_path || ""
  );

  const handleClose = () => dispatch(closeModal());

  const handleNavigation = () => {
    if (item?.id) {
      push(item?.title ? paths.movie(item.id) : paths.tvShow(item.id));
    }
  };

  return {
    t,
    title,
    overview,
    coverImage,
    posterImage,
    handleClose,
    handleNavigation,
  };
}
