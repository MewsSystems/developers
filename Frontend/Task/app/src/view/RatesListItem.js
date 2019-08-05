// @flow strict

import * as React from 'react';
import styled from 'styled-components';

import Text from '../components/Text';
import Trend from './RateTrend';

type Props = {|
  +currencyPair: string,
  +rates: number[],
|};

const Container = styled.li`
  padding-bottom: 8px;
`;

const RatesListItem = ({ currencyPair, rates }: Props) => {
  const recentRates = rates.slice(-2);

  const currentRate = recentRates[recentRates.length > 1 ? 1 : 0];
  const previousRate = recentRates.length === 2 ? recentRates[0] : null;

  return (
    <Container>
      <Text bold>
        {currencyPair}: <Text element="span">current course:</Text>{' '}
        {currentRate ? (
          <Text element="span" bold>
            {currentRate}, <Trend currentRate={currentRate} previousRate={previousRate} />
          </Text>
        ) : (
          <Text element="span">unknown</Text>
        )}
      </Text>
    </Container>
  );
};

export default RatesListItem;
