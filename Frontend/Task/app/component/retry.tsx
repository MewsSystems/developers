import React from 'react';

const RetryButton: React.FC<{reloadAction: () => any}> = (props) => (
    <div>
        <p>We are sorry, we cannot get current exchange rate at the moment.</p>
        <button onClick={props.reloadAction}>Retry</button>
    </div>
);

export default RetryButton;
