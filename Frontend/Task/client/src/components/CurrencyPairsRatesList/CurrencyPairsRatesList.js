import React from 'react';
import { isNil } from 'ramda';
import { usePrevious } from '../../hooks';

const CurrencyPairsRatesList = ({currencyPairs = {}, currencyPairsRates = {}}) => {

    const prevCurrencyPairsRates = usePrevious(currencyPairsRates);

    const list = Object.keys(currencyPairsRates).map(currencyPairId => {
        return {
            shortcut: `${currencyPairs[currencyPairId][0].code} / ${currencyPairs[currencyPairId][1].code}`,
            rate: currencyPairsRates[currencyPairId],
            prevRate: !isNil(prevCurrencyPairsRates) && prevCurrencyPairsRates[currencyPairId]
        };
    });

    return (
        <div>
            {
                list.map((it, index) => (
                    <div key={index}>
                        <span>{it.shortcut} - </span>
                        <span>{it.rate} - </span>
                        <span>
                            {
                                isNil(it.prevRate)
                                    ? null
                                    : it.prevRate < it.rate
                                    ? <span>&#x2191;</span>
                                    : it.prevRate < it.rate
                                    ? <span>&#x2193;</span>
                                    : <span>&#x3d;</span>
                            }
                        </span>
                    </div>
                ))
            }
        </div>
    );
};

export default CurrencyPairsRatesList;
