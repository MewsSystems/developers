import React, { Component } from 'react';
import { connect } from 'react-redux';
import PropTypes from 'prop-types';

import { endpoint, interval } from '../config';

import {
  deselectCurrencyPairAction,
  getConfigurationAction,
  getRatesAction,
  selectCurrencyPairAction,
  setCurrencyFilterAction,
} from '../actions';

import ErrorBoundary from '../components/ErrorBoundary';
import ExchangeRateTitle from '../components/ExchangeRateTitle';
import CurrencyPairSelector from '../components/CurrencyPairSelector';
import CurrencyPairRateList from '../components/CurrencyPairRateList';

// The ExchangeRateContainer component manages state and coordinates other components
// that render information to the user
class ExchangeRateContainer extends Component {
  state = { scrollToBottom: false };

  // get configuration and start regular polling for rates
  componentDidMount() {
    const {
      getConfiguration,
    } = this.props;
    getConfiguration(endpoint);
    this.getRatesIntervalId = setInterval(() => this.getCurrencyPairRates(), interval);
  }

  // trigger scroll to bottom of screen when new exchange rates are shown
  componentDidUpdate() {
    const { scrollToBottom } = this.state;
    /* eslint react/no-did-update-set-state: 0 */
    if (scrollToBottom) { // setState safe due to condition
      window.scrollTo({ left: 0, top: document.body.scrollHeight, behavior: 'smooth' });
      this.setState({ scrollToBottom: false });
    }
  }

  // stop polling for rates when closing page and save settings to localStorage
  componentWillUnmount() {
    clearInterval(this.getRatesIntervalId);
  }

  // get rates for all known currency pairs so they are available to display at any time
  getCurrencyPairRates() {
    const { currencyPairs, getRates } = this.props;
    const currencyPairIds = Object.keys(currencyPairs);
    getRates({ endpoint, currencyPairIds });
  }

  // return a subset of currency pair ids that match the search filter
  getFilteredCurrencyPairIds = () => {
    const {
      currencyPairs,
      filter,
    } = this.props;
    const currencyPairIds = Object.keys(currencyPairs);
    if (!filter) return currencyPairIds; // return all if the filter is empty
    const filteredCurrencyPairIds = currencyPairIds.filter((currencyPairId) => {
      const [currency1, currency2] = currencyPairs[currencyPairId];
      const { code: code1, name: name1 } = currency1;
      const { code: code2, name: name2 } = currency2;
      if (code1.toLowerCase().includes(filter.toLowerCase())) return true;
      if (code2.toLowerCase().includes(filter.toLowerCase())) return true;
      if (name1.toLowerCase().includes(filter.toLowerCase())) return true;
      if (name2.toLowerCase().includes(filter.toLowerCase())) return true;
      return false;
    });
    return filteredCurrencyPairIds;
  }

  // prepares an array of rates to display [{ shortcutName, currentValue, trend }]
  getRatesToDisplay = () => {
    const {
      currencyPairs,
      rates,
      selectedCurrencyPairs,
    } = this.props;
    const ratesToDisplay = selectedCurrencyPairs.map((currencyPairId) => {
      const name1 = currencyPairs[currencyPairId][0].name;
      const name2 = currencyPairs[currencyPairId][1].name;
      const shortcutName = `${name1}/${name2}`;
      const rateHistory = rates[currencyPairId];
      const currentValue = (rateHistory && rateHistory.length > 0)
        ? rateHistory[rateHistory.length - 1]
        : null;
      const previousValue = (rateHistory && rateHistory.length > 1)
        ? rateHistory[rateHistory.length - 2]
        : null;
      let trend = '...'; // default if there are not two values to compare
      if (currentValue && previousValue) {
        if (currentValue > previousValue) {
          trend = 'growing';
        } else if (currentValue < previousValue) {
          trend = 'declining';
        } else {
          trend = 'stagnating';
        }
      }
      return {
        shortcutName, // {name1}/{name2}
        currentValue, // current rate
        trend, // growing, declining, stagnating, ...
      };
    });
    return ratesToDisplay;
  }

  // deselect any currency pairs in the current list that are currently selected
  deselectAllVisibleCurrencyPairs = () => {
    const {
      selectedCurrencyPairs,
      deselectCurrencyPair,
    } = this.props;
    const filteredCurrencyPairIds = this.getFilteredCurrencyPairIds();
    filteredCurrencyPairIds.forEach((currencyPairId) => {
      if (selectedCurrencyPairs.includes(currencyPairId)) {
        deselectCurrencyPair(currencyPairId);
      }
    });
  }

  // select any currency pairs in the current list that are not already selected
  selectAllVisibleCurrencyPairs = () => {
    const {
      selectedCurrencyPairs,
      selectCurrencyPair,
    } = this.props;
    const filteredCurrencyPairIds = this.getFilteredCurrencyPairIds();
    filteredCurrencyPairIds.forEach((currencyPairId) => {
      if (!selectedCurrencyPairs.includes(currencyPairId)) {
        selectCurrencyPair(currencyPairId);
      }
    });
    this.setState({ scrollToBottom: true });
  }

  // update filter based on contents of input element
  handleSetCurrencyFilter = (e) => {
    const { setCurrencyFilter } = this.props;
    const { value } = e.target;
    setCurrencyFilter(value);
  }

  // select or deselect a currency pair when changing checkbox value
  toggleCurrencyPairSelection = (e) => {
    const { name: currencyPairId } = e.target;
    const {
      selectedCurrencyPairs,
      selectCurrencyPair,
      deselectCurrencyPair,
    } = this.props;
    if (selectedCurrencyPairs.includes(currencyPairId)) {
      deselectCurrencyPair(currencyPairId);
    } else {
      selectCurrencyPair(currencyPairId);
      this.setState({ scrollToBottom: true });
    }
  }

  // render presentational components
  render() {
    const {
      currencyPairs,
      errorFetchingConfiguration,
      errorFetchingRates,
      errorFetchingRatesCount,
      filter,
      isFetchingConfiguration,
      isFetchingRates,
      selectedCurrencyPairs,
    } = this.props;
    const filteredCurrencyPairIds = this.getFilteredCurrencyPairIds();
    const selectAllButtonDisabled = (filteredCurrencyPairIds.filter((currencyPairId) => {
      return !selectedCurrencyPairs.includes(currencyPairId);
    }).length === 0);
    const deselectAllButtonDisabled = (filteredCurrencyPairIds.filter((currencyPairId) => {
      return selectedCurrencyPairs.includes(currencyPairId);
    }).length === 0);
    const filteredCurrencyPairs = filteredCurrencyPairIds.reduce((acc, elem) => {
      acc[elem] = currencyPairs[elem];
      return acc;
    }, {});
    const ratesToDisplay = this.getRatesToDisplay();
    const ERROR_COUNT_THRESHOLD = 3; // can be adjusted as required
    const errorIsPersistent = (errorFetchingRatesCount > ERROR_COUNT_THRESHOLD - 1);
    return (
      <div>
        <ExchangeRateTitle />
        <ErrorBoundary>
          <CurrencyPairSelector
            currencyPairs={filteredCurrencyPairs}
            deselectAllButtonDisabled={deselectAllButtonDisabled}
            deselectAllVisibleCurrencyPairs={this.deselectAllVisibleCurrencyPairs}
            errorFetchingConfiguration={errorFetchingConfiguration}
            filter={filter}
            isFetchingConfiguration={isFetchingConfiguration}
            selectAllButtonDisabled={selectAllButtonDisabled}
            selectAllVisibleCurrencyPairs={this.selectAllVisibleCurrencyPairs}
            selectedCurrencyPairs={selectedCurrencyPairs}
            handleSetCurrencyFilter={this.handleSetCurrencyFilter}
            toggleCurrencyPairSelection={this.toggleCurrencyPairSelection}
          />
        </ErrorBoundary>
        <ErrorBoundary>
          <CurrencyPairRateList
            errorFetchingRatesCount={errorFetchingRatesCount}
            errorFetchingRates={errorFetchingRates}
            errorIsPersistent={errorIsPersistent}
            isFetchingRates={isFetchingRates}
            ratesToDisplay={ratesToDisplay}
            selectedCurrencyPairs={selectedCurrencyPairs}
          />
        </ErrorBoundary>
      </div>
    );
  }
}

ExchangeRateContainer.propTypes = {
  // Redux state
  currencyPairs: PropTypes.objectOf(PropTypes.array),
  errorFetchingConfiguration: PropTypes.string,
  errorFetchingRates: PropTypes.string,
  errorFetchingRatesCount: PropTypes.number,
  filter: PropTypes.string,
  isFetchingConfiguration: PropTypes.bool,
  isFetchingRates: PropTypes.bool,
  rates: PropTypes.objectOf(PropTypes.array),
  selectedCurrencyPairs: PropTypes.arrayOf(PropTypes.string),
  // action creators
  deselectCurrencyPair: PropTypes.func.isRequired,
  getConfiguration: PropTypes.func.isRequired,
  getRates: PropTypes.func.isRequired,
  selectCurrencyPair: PropTypes.func.isRequired,
  setCurrencyFilter: PropTypes.func.isRequired,
};
ExchangeRateContainer.defaultProps = {
  currencyPairs: {},
  errorFetchingConfiguration: '',
  errorFetchingRates: '',
  errorFetchingRatesCount: 0,
  filter: '',
  isFetchingConfiguration: false,
  isFetchingRates: false,
  rates: {},
  selectedCurrencyPairs: [],
};

const mapStateToProps = ({
  currencyPairs,
  errorFetchingConfiguration,
  errorFetchingRates,
  errorFetchingRatesCount,
  filter,
  isFetchingConfiguration,
  isFetchingRates,
  rates,
  selectedCurrencyPairs,
}) => {
  return {
    currencyPairs,
    errorFetchingConfiguration,
    errorFetchingRates,
    errorFetchingRatesCount,
    filter,
    isFetchingConfiguration,
    isFetchingRates,
    rates,
    selectedCurrencyPairs,
  };
};

const mapDispatchToProps = {
  deselectCurrencyPair: deselectCurrencyPairAction,
  getConfiguration: getConfigurationAction,
  getRates: getRatesAction,
  selectCurrencyPair: selectCurrencyPairAction,
  setCurrencyFilter: setCurrencyFilterAction,
};

export default connect(mapStateToProps, mapDispatchToProps)(ExchangeRateContainer);
