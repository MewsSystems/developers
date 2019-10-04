import React from 'react';

import styles from './ExchangeRateTitle.module.css';

// The ExchangeRateTitle component renders a simple title for the page
const ExchangeRateTitle = () => {
  return (
    <div className={styles.wrapper}>
      <h1 className={styles.title}>Exchange Rate Tracker</h1>
      <p>
        Use this page to track the current exchange rates between a selection of
        currency pairs as listed below.
      </p>
    </div>
  );
};

export default ExchangeRateTitle;
