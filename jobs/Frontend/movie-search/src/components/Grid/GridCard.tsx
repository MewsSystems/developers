import { StyledGridCard } from "./Grid.internal";

interface GridCardProps {
  children: React.ReactNode;
}

export const GridCard = (props: GridCardProps) => {
  return <StyledGridCard>{props.children}</StyledGridCard>;
};
