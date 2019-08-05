// @flow strict

import * as React from 'react';
import styled, { keyframes } from 'styled-components';

import Text from './Text';
import { COLORS } from '../utils/constants';

type Props = {|
  +children: string,
|};

const blink = keyframes`
  0% {
    opacity: 1;
  }
  100% {
    opacity: 0;
`;

const Animation = styled(Text).attrs({ element: 'span' })`
  animation: ${blink} 0.5s ease-in-out infinite alternate;
  animation-delay: ${({ delay }) => delay}s;
`;

const TextLoader = ({ children }: Props) => (
  <Text italic color={COLORS.INFO}>
    {children}
    <Animation>.</Animation>
    <Animation delay={0.2} color={COLORS.INFO}>
      .
    </Animation>
    <Animation delay={0.4} color={COLORS.INFO}>
      .
    </Animation>
  </Text>
);

export default TextLoader;
