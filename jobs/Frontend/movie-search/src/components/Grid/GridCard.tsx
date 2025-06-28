import type { ComponentPropsWithRef } from "react";
import { StyledGridCard } from "./Grid.internal";

interface GridCardProps<T> extends ComponentPropsWithRef<"div"> {
  item: T;
  // onClick: (item: T) => void;
  children: React.ReactNode;
  $isHovered: boolean;
}

export const GridCard = <T,>(props: GridCardProps<T>) => {
  return (
    <StyledGridCard
      {...props}
      $isHovered={props.$isHovered}
      // onClick={() => props.onClick(props.item)}
    >
      {props.children}
    </StyledGridCard>
  );
};
