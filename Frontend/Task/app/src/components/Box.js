// @flow strict

import * as React from 'react';
import styled from 'styled-components';

import Heading from './Heading';

type Props = {|
  +heading: string,
  +children: React.Node,
|};

const Container = styled.div`
  width: 400px;
  height: 400px;
  border: 1px solid #000;
  overflow-y: scroll;
flex-direction: column;
  padding: 16px;s
`;

const HeadingWrapper = styled.div`
  display: flex;
  justify-content: center;
  padding-bottom: 24px;
`;

const Box = ({ heading, children }: Props) => (
  <Container>
    <HeadingWrapper>
      <Heading>{heading}</Heading>
    </HeadingWrapper>
    {children}
  </Container>
);

export default Box;
