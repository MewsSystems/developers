import React from 'react';

const CurrencyPair = (props) => {
    return (
      <>
            <p>{props.currencyPair[0].code}/{props.currencyPair[1].code}</p>
      </>
    );  
}
export default CurrencyPair;

