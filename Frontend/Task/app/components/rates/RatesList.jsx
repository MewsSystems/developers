import React from 'react';
import PropTypes from 'prop-types';
import Rate from './Rate';

const renderRate = (rateData) => <Rate key={rateData.displayName} currentRate={rateData.currentRate} displayName={rateData.displayName} trend={rateData.trend} />;

const RatesList = (props) => {
  return (
    <div className="RatesList">
      {props.rates.map(renderRate)}
    </div>
  );
};

RatesList.propTypes = {
  rates: PropTypes.arrayOf(PropTypes.shape({
    currentRate: PropTypes.number,
    displayName: PropTypes.string,
    trend: PropTypes.string
  }))
}

export default RatesList;
