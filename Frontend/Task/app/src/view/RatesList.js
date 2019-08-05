// @flow strict

import * as React from 'react';
import styled from 'styled-components';
import { useSelector, useDispatch } from 'react-redux';
import qs from 'query-string';

import fetchRates from '../redux/actions/fetchRates';
import { withErrorBoundary } from '../components/ErrorBoundary';
import ListItem from './RatesListItem';

const List = styled.ul`
  list-style-type: none;
`;

const CoursesList = () => {
  const fetchConfigError = useSelector(state => state.fetchConfigError);

  if (fetchConfigError) {
    throw fetchConfigError;
  }

  const currencyPairs = useSelector(state => state.currencyPairs);
  const filteredCurrencyPairs = useSelector(state => state.filteredCurrencyPairs);

  const query = React.useMemo(
    () =>
      qs.stringify({
        currencyPairIds: currencyPairs.map(({ id }) => id),
      }),
    [currencyPairs],
  );

  const dispatch = useDispatch();
  React.useEffect(() => {
    dispatch(fetchRates(query));
  }, [dispatch, query]);

  return (
    <List>
      {currencyPairs.reduce((listItems, currencyPair) => {
        const { id, currencies, rates } = currencyPair;
        if (filteredCurrencyPairs.includes(id)) {
          return listItems;
        }

        const [firstCurrency, secondCurrency] = currencies;

        return [
          ...listItems,
          <ListItem
            key={id}
            rates={rates}
            currencyPair={`${firstCurrency.code}/${secondCurrency.code}`}
          />,
        ];
      }, [])}
    </List>
  );
};

export default withErrorBoundary<{||}>(CoursesList);
