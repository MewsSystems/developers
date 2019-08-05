// @flow strict

import * as React from 'react';
import styled from 'styled-components';
import { useSelector, useDispatch } from 'react-redux';
import qs from 'query-string';

import fetchRates from '../redux/actions/fetchRates';
import { withErrorBoundary } from '../components/ErrorBoundary';
import TextLoader from '../components/TextLoader';
import Text from '../components/Text';
import ListItem from './RatesListItem';
import { COLORS, RATES_FETCH_INTERVAL } from '../utils/constants';
import useInterval from '../utils/useInterval';

const Container = styled.div`
  display: flex;
  flex-direction: column;
  justify-content: space-between;
`;

const Info = styled.div`
  align-self: center;
  padding-top: 14px;
`;

const List = styled.ul`
  list-style-type: none;
`;

const CoursesList = () => {
  const fetchConfigError = useSelector(state => state.fetchConfigError);

  if (fetchConfigError) {
    throw fetchConfigError;
  }

  const currencyPairs = useSelector(state => state.currencyPairs);

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

  useInterval(() => {
    dispatch(fetchRates(query));
  }, RATES_FETCH_INTERVAL);

  const filteredCurrencyPairs = useSelector(state => state.filteredCurrencyPairs);
  const isLoadingRates = useSelector(state => state.isLoadingRates);
  const fetchRatesError = useSelector(state => state.fetchRatesError);

  return (
    <Container>
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
      <Info>
        {isLoadingRates && <TextLoader>Updating currency rates</TextLoader>}
        {fetchRatesError && (
          <Text color={COLORS.CRITICAL}>
            Fetching rates data failed, rates are possibly out of date!
          </Text>
        )}
      </Info>
    </Container>
  );
};

export default withErrorBoundary<{||}>(CoursesList);
