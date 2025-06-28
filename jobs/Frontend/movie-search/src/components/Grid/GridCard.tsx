import { StyledGridCard } from "./Grid.internal";

interface GridCardProps<T> {
  item: T;
  onClick: (item: T) => void;
  children: React.ReactNode;
}

export const GridCard = <T,>(props: GridCardProps<T>) => {
  return <StyledGridCard>{props.children}</StyledGridCard>;
};
