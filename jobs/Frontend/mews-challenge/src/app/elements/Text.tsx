import styled from 'styled-components';
import { Color, FontSize } from '../types';

export const Text = styled.span<{ color?: Color, size?: FontSize }>`
  color: ${props => props.color || Color['primary.dark']};
  font-size: ${props => props.size || FontSize.base};
`;

export default Text;