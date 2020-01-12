import React, { Component, } from 'react';
import {
  func, bool, shape, object, number, arrayOf,
} from 'prop-types';
import { connect, } from 'react-redux';
import { bindActionCreators, } from 'redux';

import { getRatesConfigurationAction, } from '../../actions/ratesConfigurationActions';
import { getRatesAction, } from '../../actions/ratesActions';
import RowsView from './RowsView';


class Rows extends Component {
  constructor(props) {
    super(props);

    this.ratesIntervalId = null;
  }


  componentWillUnmount() {
    this.clearRatesInterval();
  }


  handleStartRatesInterval = () => {
    this.clearRatesInterval();

    this.fetchRates();

    this.ratesIntervalId = setInterval(
      this.fetchRates,
      process.env.REACT_APP_REFETCH_INTERVAL,
    );
  }


  clearRatesInterval = () => {
    if (this.ratesIntervalId !== null) {
      clearInterval(this.ratesIntervalId);
      this.ratesIntervalId = null;
    }
  }


  fetchRates = () => {
    const { getRates, } = this.props;

    getRates();
  }


  render() {
    const {
      ratesConfigurationData,
      rows,
      rates,
      getRatesConfiguration,
    } = this.props;

    if (ratesConfigurationData.loading) {
      return (
        <tbody><tr><td>Loading</td></tr></tbody>
      );
    }


    if (ratesConfigurationData.error) {
      return (
        <tbody><tr><td>Error</td></tr></tbody>
      );
    }

    return (
      <RowsView
        key={ratesConfigurationData.timestamp}
        rows={rows}
        rates={rates}
        startRatesInterval={this.handleStartRatesInterval}
      />
    );
  }
}


const mapStateToProps = (state) => {
  const {
    data: {
      ratesConfigurationReducer,
    },
    rateListPage: {
      rateListUIReducer,
    },
  } = state;

  return {
    ratesConfigurationData: {
      loading: ratesConfigurationReducer.loading && !ratesConfigurationReducer.data,
      error: ratesConfigurationReducer.error && !ratesConfigurationReducer.data,
      timestamp: ratesConfigurationReducer.timestamp,
    },
    rows: rateListUIReducer.rows,
    rates: rateListUIReducer.rates,
  };
};

const mapDispatchToProps = (dispatch) => ({
  getRatesConfiguration: bindActionCreators(getRatesConfigurationAction, dispatch),
  getRates: bindActionCreators(getRatesAction, dispatch),
});


Rows.propTypes = {
  ratesConfigurationData: shape({
    loading: bool.isRequired,
    error: object,
    timestamp: number,
  }).isRequired,
  rows: arrayOf(object).isRequired,
  rates: object.isRequired,
  getRatesConfiguration: func.isRequired,
  getRates: func.isRequired,
};


export default connect(mapStateToProps, mapDispatchToProps)(Rows);
