// @flow
import React from 'react';
import style from './style.scss';

type Props = {
  matchExchange: Function,
  currencyPairs: Object,
  storeFilter: Function,
  filter: ?string,
};

const SearchBar = (props: Props) => {
  const { matchExchange, currencyPairs, storeFilter, filter } = props;
  return (
    <div className={style.search}>
      <input
        onKeyUp={event => {
          matchExchange(event.target.value, currencyPairs);
          storeFilter(event.target.value);
        }}
        type="text"
        className={style.searchBox}
        placeholder="Filter Rates"
        defaultValue={filter}
      />
    </div>
  );
};

export default SearchBar;
