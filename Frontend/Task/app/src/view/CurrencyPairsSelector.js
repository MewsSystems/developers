// @flow strict

import * as React from 'react';
import styled from 'styled-components';
import { useDispatch, useSelector } from 'react-redux';

import { addFilteredCurrencyPair, removeFilteredCurrencyPair } from '../redux/actions';
import { withErrorBoundary } from '../components/ErrorBoundary';
import CurrencyPairInput from './CurrencyPairInput';

const Container = styled.div`
  display: flex;
  flex-direction: column;
`;

const CurrencyPairsSelector = () => {
  const fetchConfigError = useSelector(state => state.fetchConfigError);

  if (fetchConfigError) {
    throw fetchConfigError;
  }

  const currencyPairs = useSelector(state => state.currencyPairs);
  const filteredCurrencyPairs = useSelector(state => state.filteredCurrencyPairs);

  const dispatch = useDispatch();

  const hanldeFilter = (filteredCurrencyPairId: string, isFiltered: boolean) => () => {
    if (isFiltered) {
      dispatch(removeFilteredCurrencyPair({ filteredCurrencyPairId }));
    } else {
      dispatch(addFilteredCurrencyPair({ filteredCurrencyPairId }));
    }
  };

  return (
    <Container>
      {currencyPairs.map(currencyPair => {
        const { id, currencies } = currencyPair;
        const [firstCurrency, secondCurrency] = currencies;

        return (
          <CurrencyPairInput
            key={id}
            checked={!filteredCurrencyPairs.includes(id)}
            onChange={hanldeFilter(id, filteredCurrencyPairs.includes(id))}
            label={`${firstCurrency.code}/${secondCurrency.code}`}
          />
        );
      })}
    </Container>
  );
};

export default withErrorBoundary<{||}>(CurrencyPairsSelector);
