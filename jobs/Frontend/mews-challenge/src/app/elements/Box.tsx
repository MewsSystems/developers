import styled from 'styled-components';
import { FontSize, Spacing } from '../types';

export const Box = styled.div<{
    m?: Spacing,
    p?: Spacing,
    mx?: Spacing,
    my?: Spacing,
    px?: Spacing,
    py?: Spacing,
    pt?: Spacing,
    pr?: Spacing,
    pb?: Spacing,
    pl?: Spacing,
    mt?: Spacing,
    mr?: Spacing,
    mb?: Spacing,
    ml?: Spacing,
    width?: string,
    height?: string,
    fontSize?: FontSize,
    inline?: boolean,
    textAlign?: string,
 }>`
  font-size: ${props => (props.fontSize || FontSize.base)};
  margin-top: ${props => (props.mt || props.my || props.m || Spacing.zero)};
  margin-right: ${props => (props.mr || props.mx || props.m || Spacing.zero)};
  margin-bottom: ${props => (props.mb || props.my || props.m || Spacing.zero)};
  margin-left: ${props => (props.ml || props.mx || props.m || Spacing.zero)};
  padding-top: ${props => (props.pt || props.px || props.p || Spacing.zero)};
  padding-right: ${props => (props.pr || props.py || props.p || Spacing.zero)};
  padding-bottom: ${props => (props.pb || props.py || props.p || Spacing.zero)};
  padding-left: ${props => (props.pl || props.px || props.p || Spacing.zero)};
  width: ${props => (props.width || 'auto')};
  height: ${props => (props.height || 'auto')};
  display: ${props => (props.inline ? 'inline-block' : 'block')};
  text-align: ${props => (props.textAlign || 'left')};
  position: relative;
`;

export default Box;