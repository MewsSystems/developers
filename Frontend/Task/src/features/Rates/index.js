import React, { useEffect } from 'react';
import { connect } from 'react-redux';
import { format, parse } from 'date-fns';
import { fetchConfiguration, fetchRates, setRates } from './actions';
import useInterval from '../../lib/hooks/useInterval';

import styles from './Rates.module.css';
import CurrencySelect from './CurrencySelect';
import CurrencyList from './CurrencyList';
import Spinner from '../../components/Spinner';

const Rates = ({
  fetchConfiguration,
  fetchRates,
  setRates,
  rates,
  interval,
}) => {
  const {
    currencyPairs,
    current,
    isLoadingConfig,
    isLoadingRates,
    lastUpdate,
    previous,
    selected,
  } = rates;

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
  }, interval);

  const currencyList = selected.map(({ data, label, value }) => ({
    curValue: current[value] ? current[value] : null,
    label,
    prevValue: previous[value] ? previous[value] : null,
    value,
  }));

  return (
    <article className={styles.rates}>
      <header>
        <h1>Exchange rates</h1>
      </header>

      <p>{`Last update: ${
        lastUpdate ? format(parse(lastUpdate), 'YYYY/MM/DD, hh:mm:ss') : 'Never'
      }`}</p>
      <CurrencySelect
        handleChange={setRates}
        value={selected}
        currencyPairs={currencyPairs}
      />
      <CurrencyList currencyList={currencyList} />
      {!lastUpdate && (isLoadingConfig || isLoadingRates) && <Spinner />}
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
