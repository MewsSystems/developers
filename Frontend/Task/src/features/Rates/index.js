import React, { useEffect } from 'react';
import { connect } from 'react-redux';
import { addRate, fetchConfiguration, fetchRates } from './actions';
import useInterval from '../../lib/hooks/useInterval';

const Rates = ({ addRate, fetchConfiguration, fetchRates, rates }) => {
  useEffect(() => {
    fetchConfiguration();
    rates.selected.length > 1 && fetchRates(rates.selected);
  }, [fetchConfiguration, fetchRates, rates.selected]);

  // useEffect(() => {
  //   addRate(Object.values(rates.currencyPairs)[0]);
  // }, [rates.currencyPairs]);

  useInterval(fetchConfiguration, 20000);

  const handleClick = () => {
    console.log(rates.currencyPairs);
    fetchRates([Object.keys(rates.currencyPairs)[0]]);
  };

  return (
    <div className="rates">
      <button onClick={handleClick}>abc</button>
    </div>
  );
};

const mapStateToProps = ({ rates }) => ({ rates });

const mapDispatchToProps = {
  addRate,
  fetchConfiguration,
  fetchRates,
};
export default connect(
  mapStateToProps,
  mapDispatchToProps,
)(Rates);
