import React from 'react';

const Currency = ({ currency, rate }) => {
  return <>{`${currency[0].name}/${currency[1].name}`}</>;
};

export default Currency;
