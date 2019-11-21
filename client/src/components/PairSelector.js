import React from 'react';
import {useSelector, useDispatch} from 'react-redux';
import {toggleSelectedPair} from '../actions/pairsActions';
import {Button, Row, Column, Title} from '../ui';

const PairSelector = () => (
  <Column>
    <Title>Currency pairs</Title>
    <Row>
      {useCurrencyPairs ().map (pair => (
        <Button key={pair.code} onClick={pair.toggle} filled={pair.toggled}>
          {pair.code}
        </Button>
      ))}
    </Row>
  </Column>
);

const useCurrencyPairs = () => {
  const dispatch = useDispatch ();
  const state = useSelector (state => state.pairs);

  const {currencyPairs: pairs, selectedPairs} = state;
  const selectedCodes = selectedPairs.map (e => e.code);

  return pairs.map (pair => ({
    toggled: selectedCodes.includes (pair.code),
    toggle: () => dispatch (toggleSelectedPair (pair)),
    ...pair,
  }));
};

export default PairSelector;
