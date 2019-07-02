import React, { useEffect } from 'react';
import { connect } from 'react-redux';
import * as R from 'ramda';

import { fetchConfigInit } from '../store/actions';
import Currency from './Currency';
import Trend from './Trend';

const CurrencyList = ({ fetchConfigInit, rates, config }) => {
  useEffect(() => {
    fetchConfigInit();
  }, [fetchConfigInit]);
  return (
    <div>
      {config
        ? R.keys(config).map(cur => (
            <div key={cur}>
              <Currency currency={config[cur]} />
              <Trend rate={rates[cur]} />
            </div>
          ))
        : 'not yet'}
    </div>
  );
};
const mapStateToProps = state => {
  return {
    config: state.config,
    rates: state.rates
  };
};

export default connect(
  mapStateToProps,
  { fetchConfigInit }
)(CurrencyList);
