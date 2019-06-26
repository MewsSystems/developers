import React, { useEffect, useRef } from 'react';
import { connect } from 'react-redux';
import PropTypes from 'prop-types';
import { selectCurrencyPairsIds, selectFilteredRates } from '../selectors/currencySelectors';
import ratesThunks from '../redux/thunks/ratesThunks';
import * as config from '../config.json';

import RatesList from '../components/rates/RatesList';
import RatesFilter from '../components/rates/RatesFilter';


const RatesContainer = (props) => {
  const intervalId = useRef();

  useEffect(() => {
    const callback = () => props.updateRates(props.currencyPairsIds);
    intervalId.current = setInterval(callback, config.rates.pollingInterval);
    return () => clearInterval(intervalId.current);
  }, [props.currencyPairsIds]);

  return (
    <div className="RatesContainer">
      <h1 className="RatesContainer__title">Rates</h1>
      <RatesFilter onFilter={props.filterRates} />
      <RatesList rates={props.currencyRates} />
    </div>
  );
};

RatesContainer.propTypes = {
  currencyPairsIds: PropTypes.array,
  updateRates: PropTypes.func,
  filterRates: PropTypes.func,
  currencyRates: PropTypes.array
};

const mapStateToProps = (state) => ({
  currencyPairsIds: selectCurrencyPairsIds(state),
  currencyRates: selectFilteredRates(state)
});

const mapDispatchToProps = (dispatch) => ({
  updateRates: (currencyPairsIds) => dispatch(ratesThunks.updateRates(currencyPairsIds)),
  filterRates: (filter) => dispatch(ratesThunks.filterRates(filter))
});


export default connect(mapStateToProps, mapDispatchToProps)(RatesContainer);