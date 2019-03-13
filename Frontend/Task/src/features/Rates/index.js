import React, { useEffect } from 'react';
import { connect } from 'react-redux';
import { format, parse } from 'date-fns';
import { fetchConfiguration, fetchRates, setRates } from './actions';
import useInterval from '../../lib/hooks/useInterval';

import styles from './Rates.module.css';
import CurrencySelect from './CurrencySelect';
import CurrencyList from './CurrencyList';
import Spinner from '../../components/Spinner';

const fetchSelected = (selected, fetchRates) => {
  if (selected.length > 0) {
    fetchRates(selected.map(a => a.value));
  }
};

const Rates = ({
  fetchConfiguration,
  fetchRates,
  interval,
  rates,
  setRates,
}) => {
  const {
    currencyPairs,
    current,
    isLoadingConfig,
    lastUpdate,
    previous,
    selected,
  } = rates;

  useEffect(() => {
    fetchConfiguration();
  }, [fetchConfiguration]);

  useEffect(() => {
    fetchSelected(selected, fetchRates);
  }, [fetchRates, selected]);

  useInterval(() => {
    fetchSelected(selected, fetchRates);
  }, interval);

  const currencyList = selected.map(({ label, value }) => ({
    curValue: current[value] ? current[value] : null,
    label,
    prevValue: previous[value] ? previous[value] : null,
    value,
  }));

  const hasCurrencyPairs = Object.keys(currencyPairs).length > 0;
  const hasSelected = selected.length > 0;

  return (
    <article className={styles.rates}>
      <header>
        <h1>Exchange rates</h1>
      </header>

      <p>
        Last update:{' '}
        {lastUpdate ? (
          <time dateTime={lastUpdate}>
            {format(parse(lastUpdate), 'YYYY/MM/DD, hh:mm:ss')}
          </time>
        ) : (
          'Never'
        )}
      </p>

      <CurrencySelect
        isDisabled={!hasCurrencyPairs}
        handleChange={setRates}
        value={selected}
        currencyPairs={currencyPairs}
      />

      {hasSelected && <CurrencyList currencyList={currencyList} />}

      {!hasSelected && hasCurrencyPairs && (
        <p>Please select some currencies.</p>
      )}

      {isLoadingConfig && !hasCurrencyPairs && <Spinner />}

      {!isLoadingConfig && !hasCurrencyPairs && (
        <>
          <p>It seems we don't have any currencies you could choose from.</p>
          <p>
            Could you try clicking the Reload button or reloading the page in a
            while?
          </p>
          <button
            className={styles.button}
            type="button"
            onClick={fetchConfiguration}
          >
            Reload currency pairs
          </button>
        </>
      )}
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
