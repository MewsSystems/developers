// @flow

import * as React from 'react';
import styled from 'styled-components';

const Container = styled.div`
  height: 100vh;
  display: flex;
  justify-content: center;
  align-items: center;
`;

const Box = styled.div`
  width: 400px;
  height: 400px;
  border: 1px solid #000;
  overflow-y: scroll;
`;

const View = () => (
  <Container>
    <Box />
    <Box />
  </Container>
);

export default View;
