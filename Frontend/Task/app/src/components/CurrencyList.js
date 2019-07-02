import React, { useEffect } from 'react';
import { connect } from 'react-redux';
import * as R from 'ramda';

import { fetchConfigInit } from '../store/actions';
import Currency from './Currency';
import Trend from './Trend';
import Filter from './Filter';

const CurrencyList = ({ fetchConfigInit, rates, config, filter }) => {
  useEffect(() => {
    fetchConfigInit();
  }, [fetchConfigInit]);

  const renderList = () => {
    if (config) {
      const filterApplied = filter.length;
      let CurrencyKeys = filterApplied ? filter : R.keys(config);
      return CurrencyKeys.map(cur => (
        <div key={cur}>
          <Currency currency={config[cur]} />
          <Trend rate={rates[cur]} />
        </div>
      ));
    }
  };
  return (
    <div>
      <Filter config={config} />
      {renderList()}
    </div>
  );
};
const mapStateToProps = state => {
  return {
    config: state.config,
    rates: state.rates,
    filter: state.filter
  };
};

export default connect(
  mapStateToProps,
  { fetchConfigInit }
)(CurrencyList);
