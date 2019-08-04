// @flow strict

import * as React from 'react';
import styled from 'styled-components';

type Props = {|
  +element?: 'h1' | 'h2' | 'h3' | 'h4',
  +color?: string,
  +children: React.Node,
|};

const Container = styled.h2`
  color: ${({ color }) => color || '#000'};
  font-size: 24px;
`;

const Heading = ({ element = 'h2', color, children }: Props) => (
  <Container as={element} color={color}>
    {children}
  </Container>
);

export default Heading;
