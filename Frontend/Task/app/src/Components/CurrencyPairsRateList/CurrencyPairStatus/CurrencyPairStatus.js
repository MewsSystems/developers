import React from 'react';

const CurrencyPairStatus = (props) => {
    let statusClass = props.rateStatus > 0 ? 'up' : 'down';
    statusClass = props.rateStatus === 0 ? 'stable' : statusClass;
    return (
        <p className={statusClass}></p>
    );  
}
export default CurrencyPairStatus;

