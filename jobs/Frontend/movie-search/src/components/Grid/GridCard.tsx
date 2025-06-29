import type { ComponentPropsWithRef } from "react";
import { StyledGridCard } from "./Grid.internal";

interface GridCardProps<T> extends ComponentPropsWithRef<"div"> {
  $item: T;
  children: React.ReactNode;
  $isHovered: boolean;
}

export const GridCard = <T,>(props: GridCardProps<T>) => {
  return (
    <StyledGridCard {...props} $isHovered={props.$isHovered}>
      {props.children}
    </StyledGridCard>
  );
};
