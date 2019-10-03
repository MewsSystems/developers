import React from 'react';
import PropTypes from 'prop-types';

import CurrencyPairRateItem from './CurrencyPairRateItem';

// The CurrencyPairRateList component renders a list of selected currency pairs
const CurrencyPairRateList = ({
  errorIsPersistent,
  ratesToDisplay,
}) => {
  const currencyRatesArray = ratesToDisplay.map((rate) => {
    return (
      <li key={rate.shortcutName}>
        <CurrencyPairRateItem
          rateToDisplay={rate}
        />
      </li>
    );
  });

  return (
    <div>
      {(errorIsPersistent)
        ? ''
        : (
          <ul>
            {currencyRatesArray}
          </ul>
        )}
    </div>
  );
};

CurrencyPairRateList.propTypes = {
  errorIsPersistent: PropTypes.bool.isRequired,
  ratesToDisplay: PropTypes.arrayOf(PropTypes.object).isRequired,
};

export default CurrencyPairRateList;
