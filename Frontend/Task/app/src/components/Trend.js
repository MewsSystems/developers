import React from 'react';

import IconGrow from '../assets/IconGrow';
import IconDecline from '../assets/IconDecline';
import IconStagnate from '../assets/IconStagnate';

const Trend = ({ rate }) => {
  let trendIcon = <IconStagnate />;
  if (rate) {
    if (rate.trend === 'growing') {
      trendIcon = <IconGrow />;
    }
    if (rate.trend === 'declining') {
      trendIcon = <IconDecline />;
    }
  }

  return (
    <span>
      {rate && `rate is ${rate.rate} and trend is`}
      {trendIcon}
    </span>
  );
};

export default Trend;
