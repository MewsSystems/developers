import React, { Component, } from 'react';
import {
  func, bool, shape, object, number, arrayOf, string,
} from 'prop-types';
import { connect, } from 'react-redux';
import { bindActionCreators, } from 'redux';

import { getRatesConfigurationAction, } from '../../../Main/dataActions/ratesConfigurationActions';
import { getRatesAction, } from '../../../Main/dataActions/ratesActions';
import RowsView from './RowsView';
import TableLoading from '../../../../components/Table/TableLoading';
import TableError from '../../../../components/Table/TableError';


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
    const { unfilteredRows, getRates, } = this.props;

    getRates(unfilteredRows.map((row) => row.id));
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
        <TableLoading colSpan={3} />
      );
    }


    if (ratesConfigurationData.error) {
      return (
        <TableError
          colSpan={3}
          onRefresh={getRatesConfiguration}
        />
      );
    }

    return (
      <RowsView
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
      ratesConfiguration,
    },
    rateListPage: {
      rateList,
    },
  } = state;

  return {
    ratesConfigurationData: {
      loading: ratesConfiguration.loading && !rateList.timestampConfiguration,
      error: ratesConfiguration.error && !rateList.timestampConfiguration,
    },
    rows: rateList.rows,
    unfilteredRows: rateList.unfilteredRows,
    rates: rateList.rates,
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
  unfilteredRows: arrayOf(shape({
    id: string.isRequired,
  })).isRequired,
  rates: object.isRequired,
  getRatesConfiguration: func.isRequired,
  getRates: func.isRequired,
};


export default connect(mapStateToProps, mapDispatchToProps)(Rows);
