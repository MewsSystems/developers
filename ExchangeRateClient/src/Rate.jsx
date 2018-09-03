// @flow
import React from 'react';
import styled from 'styled-components';
import type { CurrencyPairRate } from './types';

type Props = {
  className: string,
  rate: CurrencyPairRate,
};

const Wrapper = styled.div``;

const CurrencyPair = styled.div``;

const CurrencyCode = styled.span`
  font-size: 0.75rem;
  text-transform: uppercase;
  letter-spacing: 1px;
`;

const CurrencyDivider = styled.span`
  padding: 0 0.1rem;
`;

const CurrencyRate = styled.span`
  font-size: 1.5rem;
`;

const trend = (prev, curr) => {
  if (prev < curr) {
    return '↑';
  }

  if (prev > curr) {
    return '↓'
  }

  return '●';
}

const Rate = ({ className, rate }: Props) => {
  const [prev, curr] = rate.rate;
  const { left, right } = rate.currencyPair;

  return (
    <Wrapper className={className}>
      <CurrencyPair>
        <CurrencyCode>{left.code}</CurrencyCode>
        <CurrencyDivider>/</CurrencyDivider>
        <CurrencyCode>{right.code}</CurrencyCode>
      </CurrencyPair>
      <CurrencyRate>{curr} {trend(prev, curr)}</CurrencyRate>
    </Wrapper>
  )
};

Rate.defaultProps = {
  className: '',
};

export default Rate;
