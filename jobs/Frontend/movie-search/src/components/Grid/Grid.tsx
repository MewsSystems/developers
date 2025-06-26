import { StyledGrid } from "./Grid.internal";

interface GridProps {
  children: React.ReactNode;
}

export const Grid = (props: GridProps) => {
  return <StyledGrid>{props.children}</StyledGrid>;
};
