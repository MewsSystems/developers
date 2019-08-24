import React from 'react';

const CurrencyPairRate = (props: {rateValue: number}) => {
  const { rateValue } = props;
  return (
    <>
      <p>{rateValue}</p>
    </>
  );  
}

export default CurrencyPairRate;

