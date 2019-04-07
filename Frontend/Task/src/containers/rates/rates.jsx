import React, { Component } from 'react';
import PropTypes from 'prop-types';
import { connect } from 'react-redux';

import CurrencySelector from '../../components/currency-selector';
import RatesList from '../../components/rates-list';

import { getRates, setRates } from '../../actions/rates';
import { getRatesFromApi } from '../../helpers/common';

import styles from './rates.module.scss';
import getCurrenciesList, { getCurrencyPairs } from './selectors';

class Rates extends Component {
  componentDidMount() {
    this.timerId = setTimeout(function tick() {
      const {
        selected,
        getRates: getSelectedRates,
      } = this.props;

      getRatesFromApi(selected, getSelectedRates);

      this.timerId = setTimeout(tick.bind(this), 10000);
    }.bind(this), 0);
  }

  componentWillUnmount() {
    clearTimeout(this.timerId);
  }

  render() {
    const {
      currencyPairs,
      selected,
      setRates: setSelectedRates,
      currenciesList,
    } = this.props;

    const hasSelectedPairs = selected && selected.length > 0;

    return (
      <>
        <div className={styles.currencySelectorWrapper}>
          <CurrencySelector
            currencies={currencyPairs}
            value={selected}
            isDisabled={!currencyPairs}
            handleChange={setSelectedRates}
          />
        </div>

        <div className={styles.ratesListWrapper}>
          {hasSelectedPairs && <RatesList items={currenciesList} />}

          {!hasSelectedPairs && currencyPairs && (
            <h3>Please select currencies to display rates</h3>
          )}
        </div>
      </>
    );
  }
}

Rates.defaultProps = {
  selected: [],
  currencyPairs: null,
};

Rates.propTypes = {
  currencyPairs: PropTypes.object,
  currenciesList: PropTypes.array.isRequired,
  selected: PropTypes.array,
  setRates: PropTypes.func.isRequired,
  getRates: PropTypes.func.isRequired,
};

/**
 * Mapping state values to container props
 * @param state
 */
const mapStateToProps = state => ({
  currencyPairs: getCurrencyPairs(state),
  currenciesList: getCurrenciesList(state),
  ...state.rates,
});

const mapDispatchToProps = {
  getRates,
  setRates,
};


export default connect(mapStateToProps, mapDispatchToProps)(Rates);
