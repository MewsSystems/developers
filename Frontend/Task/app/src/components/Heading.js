// @flow strict

import * as React from 'react';
import styled from 'styled-components';

import { COLORS } from '../utils/constants';

type Props = {|
  +element?: 'h1' | 'h2' | 'h3' | 'h4',
  +color?: string,
  +children: React.Node,
|};

const Container = styled.h2`
  color: ${({ color }) => color || COLORS.DEFAULT};
  font-size: 24px;
`;

const Heading = ({ element = 'h2', color, children }: Props) => (
  <Container as={element} color={color}>
    {children}
  </Container>
);

export default Heading;
