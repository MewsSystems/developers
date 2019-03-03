import React, { useEffect } from 'react';
import { connect } from 'react-redux';
import { fetchConfiguration, fetchRates, setRates } from './actions';
import useInterval from '../../lib/hooks/useInterval';

import CurrencySelect from './CurrencySelect';
import CurrencyList from './CurrencyList';

const Rates = ({ fetchConfiguration, fetchRates, setRates, rates }) => {
  const { currencyPairs, current, previous, selected } = rates;

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
  }, 2000);

  const getTrend = (prevValue, nextValue) => {
    if (!prevValue || !nextValue) {
      return 'unknown';
    }
    if (prevValue < nextValue) {
      return 'up';
    } else if (prevValue > nextValue) {
      return 'down';
    } else if (prevValue === nextValue) {
      return 'equal';
    }
    return 'unknown';
  };

  const pairToLabel = currencyPair => {
    const [from, to] = currencyPair;
    return `${from.code}/${to.code} - ${from.name}/${to.name}`;
  };

  // TODO: Don't return duplicates - from, label?
  const pairsToOptions = currencyPairs => {
    return Object.keys(currencyPairs).reduce((acc, cur) => {
      const [from, to] = currencyPairs[cur];
      return [
        ...acc,
        {
          value: cur,
          label: pairToLabel(currencyPairs[cur]),
          from,
          to,
        },
      ];
    }, []);
  };

  const currencyList = selected.map(item => ({
    ...item,
    trend: getTrend(
      previous[item.value] ? previous[item.value] : null,
      current[item.value] ? current[item.value] : null,
    ),
  }));

  return (
    <div className="rates">
      <CurrencySelect
        handleChange={setRates}
        value={selected}
        options={pairsToOptions(currencyPairs)}
      />
      <CurrencyList currencyList={currencyList} />
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
