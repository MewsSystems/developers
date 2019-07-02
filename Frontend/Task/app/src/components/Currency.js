import React from 'react';

import { CurrencyItem } from '../assets/Styles';

const Currency = ({ currency, rate }) => {
  return (
    <CurrencyItem>{`${currency[0].name}/${currency[1].name}`}</CurrencyItem>
  );
};

export default Currency;
