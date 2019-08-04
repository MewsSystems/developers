// @flow strict

import * as React from 'react';
import styled from 'styled-components';
import { useSelector } from 'react-redux';

import { withErrorBoundary } from '../components/ErrorBoundary';
import ListItem from './CoursesListItem';

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

  return (
    <List>
      {currencyPairs.reduce((listItems, currencyPair) => {
        const { id, currencies } = currencyPair;
        if (filteredCurrencyPairs.includes(id)) {
          return listItems;
        }

        const [firstCurrency, secondCurrency] = currencies;

        return [
          ...listItems,
          <ListItem
            key={id}
            courses={[]}
            currencyPair={`${firstCurrency.code}/${secondCurrency.code}`}
          />,
        ];
      }, [])}
    </List>
  );
};

export default withErrorBoundary<{||}>(CoursesList);
