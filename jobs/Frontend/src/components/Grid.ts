import styled from 'styled-components';
import {
  grid,
  justifyContent,
  justifyItems,
  alignContent,
  alignItems,
  GridProps as GridStyledProps,
  JustifyContentProps,
  JustifyItemsProps,
  AlignContentProps,
  AlignItemsProps,
  compose,
} from '@tradersclub/styled-system';
import { placeItems, PlaceItemsProps } from '@/lib/system/placeItems';
import Box from '@/components/Box';

const styledSystemProps = [
  ...(grid.propNames ?? []),
  ...(justifyContent.propNames ?? []),
  ...(justifyItems.propNames ?? []),
  ...(alignContent.propNames ?? []),
  ...(alignItems.propNames ?? []),
  ...(placeItems.propNames ?? []),
];

export interface GridProps extends GridStyledProps, PlaceItemsProps, JustifyContentProps, JustifyItemsProps, AlignContentProps, AlignItemsProps, PlaceItemsProps {
  $inline?: boolean;
}

export const Grid = styled(Box).withConfig({
  shouldForwardProp: (prop) => !styledSystemProps.includes(prop),
})<GridProps>`
  display: grid;
  ${compose(
  grid,
  justifyContent,
  justifyItems,
  alignContent,
  alignItems,
  placeItems,
)}
`;
export default Grid;
