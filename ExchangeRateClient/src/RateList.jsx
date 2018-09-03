// @flow
import React from 'react';
import { List } from 'immutable';
import styled from 'styled-components';

import type { CurrencyPairRate } from './types';
import Rate from './Rate';

type Props = {
  className: string,
  rates: List<CurrencyPairRate>,
};

const StyledRate = styled(Rate)`
  margin: 0 0 0.5rem;
`;

const RateList = ({ className, rates }: Props) => (
  <div className={className}>
    {rates.map(rate => (
      <StyledRate key={rate.id} rate={rate} />
    ))}
  </div>
);

RateList.defaultProps = {
  className: '',
};

export default RateList;
