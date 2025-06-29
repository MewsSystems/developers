import React from "react";
import { GridWrapper, StyledGrid } from "./Grid.internal";

interface GridProps<T> {
  items: T[];
  renderItem: (item: T) => React.ReactNode;
  keyExtractor: (item: T, index: number) => React.Key;
}

export const Grid = <T,>(props: GridProps<T>) => {
  return (
    <GridWrapper>
      <StyledGrid>
        {Array.from(props.items, (item, index) => (
          <React.Fragment
            key={props.keyExtractor ? props.keyExtractor(item, index) : index}
          >
            {props.renderItem(item)}
          </React.Fragment>
        ))}
      </StyledGrid>
    </GridWrapper>
  );
};
