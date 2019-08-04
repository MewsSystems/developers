// @flow strict

import * as React from 'react';
import styled, { keyframes } from 'styled-components';

import { COLORS } from '../utils/constants';

const animation = keyframes`
  0% {
    top: 28px;
    left: 28px;
    width: 0;
    height: 0;
    opacity: 1;
  }
  100% {
    top: -1px;
    left: -1px;
    width: 58px;
    height: 58px;
    opacity: 0;
  }`;

const Container = styled.div`
  display: inline-block;
  position: absolute;
  top: 50%;
  left: 50%;
  width: 64px;
  height: 64px;
  transform: translate(-50%, -50%);

  & div {
    position: absolute;
    border: 4px solid ${COLORS.LOADER};
    opacity: 1;
    border-radius: 50%;
    animation: ${animation} 1s cubic-bezier(0, 0.2, 0.8, 1) infinite;
  }

  & .delayed {
    animation-delay: -0.5s;
  }
`;

const Loader = () => (
  <Container>
    <div />
    <div className="delayed" />
  </Container>
);

export default Loader;
