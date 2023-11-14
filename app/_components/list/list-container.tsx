import { FC } from "react";
import { Title } from "@/styles/base/title";
import { Movie } from "@/types/movie";
import { TvShow } from "@/types/tv-show";
import { List } from "./list";
import { ListContainer as StyledListContainer } from "@/styles/components/list-container";

type Props = {
  title: string;
  items: Movie[] | TvShow[];
};

export const ListContainer: FC<Props> = (props) => {
  const { title, items } = useListContainer(props);

  return (
    <StyledListContainer $gap="lg">
      <Title>{title}</Title>
      <List items={items} />
    </StyledListContainer>
  );
};

function useListContainer({ title, items }: Props) {
  return { title, items };
}
