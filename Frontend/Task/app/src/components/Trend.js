import React from 'react';

const Trend = ({ rate }) => {
  return (
    <span>{rate && `rate is ${rate.rate} and trend is ${rate.trend}`}</span>
  );
};

export default Trend;
