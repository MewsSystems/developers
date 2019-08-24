import React from 'react';

const CurrencyPairStatus = (props: {rateStatus: number}) => {
  const { rateStatus } = props;
  let statusClass = rateStatus > 0 ? 'up' : 'down';
  statusClass = rateStatus === 0 ? 'stable' : statusClass;
  return (
    <p className={statusClass}></p>
  );  
}

export default CurrencyPairStatus;

