import React from 'react';

export const CurrencyPairsRateList = ({ currencyPair }) => {

  return (
    <div>
      {currencyPair.display && currencyPair.name}
    </div>
  )
}

export default CurrencyPairsRateList
