import React, {ReactElement} from 'react';
import CurrencyPairInterface from '../interface/currencyPairInterface';

const CurrencyPair: React.FC<CurrencyPairInterface> = (
    { pair, shortcut, rate, trend },
): ReactElement<any> => (
    <li className="pair">
        <strong>{shortcut}</strong>
        <span>{rate && rate}</span>
        <span>{trend && trend}</span>
    </li>
);

export default CurrencyPair;
