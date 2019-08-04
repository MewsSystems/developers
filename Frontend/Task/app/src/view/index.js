// @flow strict

import * as React from 'react';
import styled from 'styled-components';

import Box from '../components/Box';
import CurrencyPairsSelector from './CurrencyPairsSelector';
import CoursesList from './CoursesList';

const Container = styled.div`
  height: 100vh;
  display: flex;
  justify-content: center;
  align-items: center;
`;

const View = () => (
  <Container>
    <Box heading="Filter currency pairs">
      <CurrencyPairsSelector />
    </Box>
    <Box heading="Currency courses">
      <CoursesList />
    </Box>
  </Container>
);

export default View;
