import React, { ReactElement } from 'react';
import CurrencyPair from './currencyPair';
import CurrencyPairInterface from '../interface/currencyPairInterface';
import { iteratePairCollection } from '../action/currencyPair';
import PairInterface from '../interface/pairInterface';
import CurrencyPairListStyle from '../style/currencyPairList';

const getPair = (key: string, currencyPair: CurrencyPairInterface, selectedPair: string): ReactElement<any> => {
    const { shortcut, rate, trend, pair } = currencyPair;

    return (!selectedPair || selectedPair === key)
        ? (
            <CurrencyPair key={key} pair={pair} shortcut={shortcut} rate={rate} trend={trend}/>
        )
        : null;
};

const CurrencyPairList: React.FC<{pairs: PairInterface, selectedPair: string}> = (
    {pairs, selectedPair},
): ReactElement<any> => {
    return (
        <CurrencyPairListStyle>
            <li>
                <strong>currencies</strong>
                <span>exchange rate</span>
                <span>trend</span>
            </li>
            {pairs && iteratePairCollection(pairs, (key, currencyPair) => getPair(key, currencyPair, selectedPair))}
        </CurrencyPairListStyle>
    );
};

export default CurrencyPairList;
