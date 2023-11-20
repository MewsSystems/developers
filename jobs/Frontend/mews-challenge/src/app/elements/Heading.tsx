import styled from 'styled-components';
import { Color, FontSize } from '../types';

export const Heading = styled.h1<{ color?: Color, size?: FontSize, textAlign?: string }>`
  color: ${props => props.color || Color['primary.dark']};
  font-size: ${props => props.size || FontSize.base};
  text-align: ${props => props.textAlign || "left"};
`;

export default Heading;