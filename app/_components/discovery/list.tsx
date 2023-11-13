import { FC } from "react";
import { Box } from "@/styles/base/box";
import { Movie } from "@/types/movie";
import { TvShow } from "@/types/tv-show";
import { Swiper, SwiperSlide } from "swiper/react";
import { ListItem } from "./list-item";

type Props = {
  items: Movie[] | TvShow[];
};

export const List: FC<Props> = (props) => {
  const { items } = useList(props);

  const renderItem = (item: Movie | TvShow) => (
    <SwiperSlide key={item.id}>
      <ListItem item={item} />
    </SwiperSlide>
  );

  return (
    <Box>
      <Swiper grabCursor={true} spaceBetween={20} slidesPerView="auto">
        {items.map(renderItem)}
      </Swiper>
    </Box>
  );
};

function useList({ items }: Props) {
  return { items };
}
