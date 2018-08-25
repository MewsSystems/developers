// @flow
import React from 'react';
import { StoreFilter, GetFilter } from 'Utils/localStorage';

import SearchBar from 'Components/searchBar';
import style from './style.scss';

type Props = {
  counter: number,
  matchExchange: Function,
  currencyPairs: Object,
};

const Header = (props: Props) => {
  const { counter, matchExchange, currencyPairs } = props;
  return (
    <header className={style.header}>
      <div className={style.name}>
        <span role="img" aria-label="Icon">
          💰
        </span>
      </div>
      <SearchBar
        matchExchange={matchExchange}
        currencyPairs={currencyPairs}
        storeFilter={StoreFilter}
        filter={GetFilter()}
      />
      <div className={style.countdown}>{counter}</div>
    </header>
  );
};

export default Header;
