import { apiConfig } from "@/domain/remote/config";
import { PosterItem, PostersWrapper } from "@/styles/components/posters";
import { Image } from "@/types/image";
import { useTranslations } from "next-intl";
import { FC } from "react";
import { Swiper, SwiperSlide } from "swiper/react";

type Props = {
  images: Image[];
};

export const PostersList: FC<Props> = ({ images }) => {
  const t = useTranslations("movie");

  const renderPoster = (poster: Image) => (
    <SwiperSlide key={poster.file_path} style={{ width: "auto" }}>
      <a href={apiConfig.coverImage(poster.file_path)} target="_blank">
        <PosterItem $bgImage={apiConfig.coverImage(poster.file_path)} />
      </a>
    </SwiperSlide>
  );

  return (
    <PostersWrapper $mt="xl">
      <Swiper grabCursor={true} spaceBetween={15} slidesPerView="auto">
        {images.map(renderPoster)}
      </Swiper>
    </PostersWrapper>
  );
};
