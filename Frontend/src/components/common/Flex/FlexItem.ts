import styled from 'styled-components';

const FlexItem = styled.div<FlexItemProps>`
  order: ${({ order }) => order || 0};
  flex-basis: ${({ basis }) => basis || 'auto'};
  flex-grow: ${({ grow }) => grow || 0};
  flex-shrink: ${({ noShrink, shrink }) => (noShrink ? 0 : shrink || 1)};
  display: ${({ $display }) => $display || 'block'};
`;

interface BaseProps {
  order?: number;
  $display?: 'flex' | 'inline-flex' | 'inline-block';
  basis?: number | string;
  grow?: number;
}

interface FlexItemShrinkProps extends BaseProps {
  shrink?: number;
  noShrink?: never;
}

interface FlexItemNoShrinkProps extends BaseProps {
  noShrink?: boolean;
  shrink?: never;
}

export type FlexItemProps = FlexItemShrinkProps | FlexItemNoShrinkProps;
export default FlexItem;
