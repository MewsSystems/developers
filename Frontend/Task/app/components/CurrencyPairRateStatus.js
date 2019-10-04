import React from 'react';
import PropTypes from 'prop-types';

import styles from './CurrencyPairRateStatus.module.css';

// The CurrencyPairRateStatus component renders messages and status indicators
// to inform the user of the current status of the exchange rate service
const CurrencyPairRateStatus = ({
  errorFetchingRatesCount,
  errorFetchingRates,
  errorIsPersistent,
  isFetchingRates,
  ratesAreAvailable,
  selectedCurrencyPairsCount,
}) => {
  const loadingMessage = (selectedCurrencyPairsCount > 0 && !ratesAreAvailable)
    // render loading message if request rates are not available to display
    ? <p className={styles.loading}>Loading rates, please wait.</p>
    : '';
  const errorMessage = (errorIsPersistent) // render error message if repeated API failure
    ? (
      <div className={styles.error}>
        <p>
          Sorry, there was an ongoing problem loading the current exchange rates.
          They will reappear if contact with the server is re-established.
        </p>
        <p>
          {`The error received was "${errorFetchingRates}".`}
        </p>
      </div>
    )
    : '';

  const greenDot = <span className={styles.greenDot} />;
  const yellowDot = <span className={styles.yellowDot} />;
  const redDot = <span className={styles.redDot} />;
  const valueStatus = (ratesAreAvailable) ? greenDot : yellowDot;
  const errorStatus = (errorFetchingRatesCount === 0)
    ? valueStatus
    : Array(Math.min(errorFetchingRatesCount, 5)).fill(redDot).map((dot, index) => {
      /* eslint react/no-array-index-key: 0 */ // cancel warning as dots are identical
      return <span key={index}>{dot}</span>;
    });
  const fetchingStatus = (isFetchingRates) ? yellowDot : '';

  return (
    <div className={styles.wrapper}>
      {loadingMessage}
      {errorMessage}
      <p className={styles.status}>
        service status:
        {errorStatus}
        {fetchingStatus}
      </p>
    </div>
  );
};

CurrencyPairRateStatus.propTypes = {
  errorFetchingRatesCount: PropTypes.number.isRequired,
  errorFetchingRates: PropTypes.string.isRequired,
  errorIsPersistent: PropTypes.bool.isRequired,
  isFetchingRates: PropTypes.bool.isRequired,
  ratesAreAvailable: PropTypes.bool.isRequired,
  selectedCurrencyPairsCount: PropTypes.number.isRequired,
};

export default CurrencyPairRateStatus;
