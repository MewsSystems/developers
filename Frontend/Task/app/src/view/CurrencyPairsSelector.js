// @flow strict

import * as React from 'react';
import styled from 'styled-components';

import CurrencyPairInput from './CurrencyPairInput';

const Container = styled.div`
  display: flex;
  flex-direction: column;
`;

const CurrencyPairsSelector = () => {
  return (
    <Container>
      <CurrencyPairInput checked onChange={() => {}}>
        ABC/EFG
      </CurrencyPairInput>
      <CurrencyPairInput checked onChange={() => {}}>
        ABC/EFG
      </CurrencyPairInput>
      <CurrencyPairInput checked onChange={() => {}}>
        ABC/EFG
      </CurrencyPairInput>
      <CurrencyPairInput checked={false} onChange={() => {}}>
        ABC/EFG
      </CurrencyPairInput>
    </Container>
  );
};

export default CurrencyPairsSelector;
