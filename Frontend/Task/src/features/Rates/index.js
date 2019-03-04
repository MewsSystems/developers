import React, { useEffect } from 'react';
import { connect } from 'react-redux';
import { fetchConfiguration, fetchRates, setRates } from './actions';
import useInterval from '../../lib/hooks/useInterval';

import styles from './Rates.module.css';
import CurrencySelect from './CurrencySelect';
import CurrencyList, {
  TREND_UP,
  TREND_DOWN,
  TREND_EQUAL,
  TREND_UNKNOWN,
} from './CurrencyList';
import Spinner from '../../components/Spinner';

const Rates = ({
  fetchConfiguration,
  fetchRates,
  setRates,
  rates,
  updateInterval,
}) => {
  const { currencyPairs, current, previous, selected, configStatus } = rates;

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
  }, updateInterval);

  const pairToLabel = currencyPair => {
    const [from, to] = currencyPair;
    return `${from.code}/${to.code}`;
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

  const getTrend = (prevValue, nextValue) => {
    if (!prevValue || !nextValue) {
      return TREND_UNKNOWN;
    }
    if (prevValue < nextValue) {
      return TREND_UP;
    } else if (prevValue > nextValue) {
      return TREND_DOWN;
    } else if (prevValue === nextValue) {
      return TREND_EQUAL;
    }
    return TREND_UNKNOWN;
  };

  const currencyList = selected.map(item => ({
    ...item,
    trend: getTrend(
      previous[item.value] ? previous[item.value] : null,
      current[item.value] ? current[item.value] : null,
    ),
  }));

  if (currencyPairs.length === 0) {
    return 'Sorry, there are no currency pairs...';
  }

  // if (configStatus.isLoading) {
  //   return <Spinner />;
  // }

  return (
    <article className={styles.rates}>
      <header>
        <h1>Exchange rates</h1>
      </header>
      <CurrencySelect
        handleChange={setRates}
        value={selected}
        options={pairsToOptions(currencyPairs)}
      />
      <CurrencyList currencyList={currencyList} />
    </article>
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
