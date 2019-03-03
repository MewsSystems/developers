import React, { useEffect } from 'react';
import { connect } from 'react-redux';
import { fetchConfiguration, fetchRates, setRates } from './actions';
import useInterval from '../../lib/hooks/useInterval';

import CurrencySelect from './CurrencySelect';

const Rates = ({
  addRate,
  fetchConfiguration,
  fetchRates,
  setRates,
  rates,
}) => {
  const { currencyPairs, selected } = rates;

  useEffect(() => {
    fetchConfiguration();
  }, [fetchConfiguration]);

  useEffect(() => {
    if (selected.length > 0) {
      fetchRates(selected.map(a => a.value));
    }
  }, [fetchRates, selected]);

  useInterval(() => {
    if (selected.length > 0) {
      fetchRates(selected.map(a => a.value));
    }
  }, 20000);

  const pairToLabel = currencyPair => {
    const [from, to] = currencyPair;
    return `${from.code}/${to.code} - ${from.name}/${to.name}`;
  };

  const pairsToOptions = currencyPairs => {
    return Object.keys(currencyPairs).reduce((acc, cur) => {
      return [
        ...acc,
        {
          value: cur,
          label: pairToLabel(currencyPairs[cur]),
        },
      ];
    }, []);
  };

  return (
    <div className="rates">
      <CurrencySelect
        handleChange={setRates}
        value={selected}
        options={pairsToOptions(currencyPairs)}
      />
    </div>
  );
};

const mapStateToProps = ({ rates }) => ({ rates });

const mapDispatchToProps = {
  fetchConfiguration,
  fetchRates,
  setRates,
};
export default connect(
  mapStateToProps,
  mapDispatchToProps,
)(Rates);
