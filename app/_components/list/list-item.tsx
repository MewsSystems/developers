import { FC } from "react";
import {
  ListItemTitle,
  ListItem as StyledListItem,
} from "@/styles/components/list-item";
import { Movie } from "@/types/movie";
import { TvShow } from "@/types/tv-show";
import { apiConfig } from "@/domain/remote/config";
import { useDispatch } from "react-redux";
import { openModal } from "@/store/slices/item-modal";

type Props = {
  item: Movie | TvShow;
};

export const ListItem: FC<Props> = (props) => {
  const { item, title, image } = useListItem(props);

  const dispatch = useDispatch();

  return (
    <StyledListItem
      $bgImage={image}
      onClick={() => {
        dispatch(openModal(item as Movie & TvShow));
      }}
    >
      <ListItemTitle $variant="h3">{title}</ListItemTitle>
    </StyledListItem>
  );
};

function useListItem({ item }: Props) {
  const title = (item as Movie).title || (item as TvShow).name;
  const coverImage = apiConfig.coverImage(
    item.backdrop_path || item.poster_path
  );
  const posterImage = apiConfig.posterImage(
    item.poster_path || item.backdrop_path
  );

  return { item, title, image: coverImage || posterImage };
}
