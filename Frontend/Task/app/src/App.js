import React, { useEffect } from 'react';
import { connect } from 'react-redux';
import { fetchConfigInit } from './store/actions';

import * as R from 'ramda';

const App = ({ fetchConfigInit, rates, config }) => {
  useEffect(() => {
    fetchConfigInit();
  }, [fetchConfigInit]);

  return (
    <div>
      Exchange rates App
      {config
        ? R.keys(config).map(cur => (
            <p key={cur}>
              {config[cur][0].name}/{config[cur][1].name}{' '}
              {rates[cur]
                ? `${rates[cur].rate} trnd is ${rates[cur].trend}`
                : null}
            </p>
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
)(App);
