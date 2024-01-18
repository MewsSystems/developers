import styled from 'styled-components';
import { space, position, PositionProps, SpaceProps, layout, LayoutProps, GridAreaProps, gridArea, compose, alignSelf, AlignSelfProps } from '@tradersclub/styled-system';
import { aspectRatio, AspectRatioProps } from '@/lib/system/aspectRatio';

const styledSystemProps = [
  ...(space.propNames ?? []),
  ...(layout.propNames ?? []),
  ...(position.propNames ?? []),
  ...(gridArea.propNames ?? []),
  ...(aspectRatio.propNames ?? []),
  ...(alignSelf.propNames ?? []),
];

export type BoxProps = SpaceProps & PositionProps & LayoutProps & GridAreaProps & AspectRatioProps & AlignSelfProps;

export const Box = styled.div.withConfig({
  shouldForwardProp: (prop) => !styledSystemProps.includes(prop),
})<BoxProps>`
  ${compose(
  space,
  position,
  layout,
  gridArea,
  aspectRatio,
)}
`;

export default Box;
