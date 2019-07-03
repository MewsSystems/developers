import React from 'react';

import IconGrow from '../assets/Icons/IconGrow';
import IconDecline from '../assets/Icons/IconDecline';
import IconStagnate from '../assets/Icons/IconStagnate';
import { CurrencyTrend } from '../assets/Styles';
import { RateInterface } from '../types';

const Trend: React.FC<RateInterface> = ({ rate }) => {
  let trendIcon = <IconStagnate />;
  if (rate) {
    if (rate.trend === 'growing') {
      trendIcon = <IconGrow />;
    }
    if (rate.trend === 'declining') {
      trendIcon = <IconDecline />;
    }
  }
  if (rate) {
    return (
      <CurrencyTrend direction={rate.trend}>
        <span>{rate.rate}</span>
        {trendIcon}
      </CurrencyTrend>
    );
  }
  return null;
};

export default Trend;
