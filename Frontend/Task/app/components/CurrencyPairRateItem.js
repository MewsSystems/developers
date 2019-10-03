import React from 'react';
import PropTypes from 'prop-types';

import styles from './CurrencyPairRateItem.module.css';

// The CurrencyPairRateItem component renders rate information for a specific currency pair
const CurrencyPairRateItem = ({ rateToDisplay }) => {
  const {
    shortcutName,
    currentValue,
    trend,
  } = rateToDisplay;
  const valueToDisplay = (currentValue)
    ? currentValue.toString()
    : '...';
  let trendStyle = styles.trend;
  if (trend === 'growing') trendStyle = `${styles.trend} ${styles.growing}`;
  if (trend === 'declining') trendStyle = `${styles.trend} ${styles.declining}`;
  return (
    <div className={styles.wrapper}>
      <span className={styles.shortcutName}>{shortcutName}</span>
      <span className={styles.currentValue}>{valueToDisplay}</span>
      <span className={trendStyle}>{trend}</span>
    </div>
  );
};

CurrencyPairRateItem.propTypes = {
  rateToDisplay: PropTypes.shape({
    shortcutName: PropTypes.string,
    currentValue: PropTypes.number,
    trend: PropTypes.string,
  }).isRequired,
};

export default CurrencyPairRateItem;
