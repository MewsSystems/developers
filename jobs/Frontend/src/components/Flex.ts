import styled from 'styled-components';
import {
  compose,
  flexbox,
  FlexboxProps,
  GridAreaProps,
  gridColumnGap,
  GridColumnGapProps,
  gridGap,
  GridGapProps,
  gridRowGap,
  GridRowGapProps,
} from '@tradersclub/styled-system';
import { placeItems, PlaceItemsProps } from '@/lib/system/placeItems';
import Box from '@/components/Box';

const styledSystemProps = [
  ...(flexbox.propNames ?? []),
  ...(gridGap.propNames ?? []),
  ...(gridColumnGap.propNames ?? []),
  ...(gridRowGap.propNames ?? []),
  ...(placeItems.propNames ?? []),
];

export interface FlexProps extends FlexboxProps, GridGapProps, GridColumnGapProps, GridRowGapProps, GridAreaProps, PlaceItemsProps {
  $inline?: boolean;
}

export const Flex = styled(Box).withConfig({
  shouldForwardProp: (prop) => {
    return !styledSystemProps.includes(prop);
  },
})<FlexProps>`
  display: ${({ $inline }) => $inline ? 'inline-flex' : 'flex'};
  ${compose(
  flexbox,
  gridGap,
  gridColumnGap,
  gridRowGap,
  placeItems,
)}
`;

export default Flex;
