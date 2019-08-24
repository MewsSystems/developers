import React from 'react';
import ICurrencyPairs from '../../../interfaces/CurrencyPairs.interface';

const CurrencyPair = (props: ICurrencyPairs) => {
  const { currencyPair } = props;
  return (
    <>
      <p>{currencyPair[0].code}/{currencyPair[1].code}</p>
    </>
  );  
}
export default CurrencyPair;

