// @flow strict

import * as React from 'react';
import styled from 'styled-components';

import { SIZES, COLORS } from '../utils/constants';

type Props = {|
  +element?: 'span' | 'div' | 'p',
  +color?: string,
  +bold?: boolean,
  +italic?: boolean,
  +size?: $Keys<typeof SIZES>,
  +children: React.Node,
|};

const Container = styled.p`
  color: ${({ color }) => color || COLORS.DEFAULT};
  font-size: ${({ size }) => SIZES[size]};
  font-weight: ${({ bold }) => (bold ? 'bold' : 'normal')};
  font-style: ${({ italic }) => (italic ? 'italic' : 'normal')};
`;

const Text = ({ element = 'p', color, bold, italic, size = 'normal', children }: Props) => (
  <Container as={element} color={color} bold={bold} otalic={italic} size={size}>
    {children}
  </Container>
);

export default Text;
