import React from 'react';
import PropTypes from 'prop-types';

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
  return (
    <div>
      <span>{shortcutName}</span>
      <span>{valueToDisplay}</span>
      <span>{trend}</span>
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
