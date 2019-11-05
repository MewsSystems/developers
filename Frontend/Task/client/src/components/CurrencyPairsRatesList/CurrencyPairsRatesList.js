import React from 'react';
import { isNil } from 'ramda';
import { usePrevious } from '../../hooks';
import styles from './CurrencyPairsRatesList.module.css';
import classNames from 'classnames';

// App passes to this component always a non-empty currencyPairsRates array
// if fetch returns empty array or error, this component doesn't receive a new currencyPairsRates prop

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
        <div
            className={styles['table']}
        >
            <div
                className={classNames(
                    styles['table-row'],
                    styles['table-row--header'],
                )}
            >
                <div
                    className={classNames(
                        styles['table-column'],
                        styles['table-column--shortcut'],
                    )}
                >
                    <span>Currency Pair</span>
                </div>
                <div
                    className={classNames(
                        styles['table-column'],
                        styles['table-column--rate'],
                    )}
                >
                    <span>Rate</span>
                </div>
                <div
                    className={classNames(
                        styles['table-column'],
                        styles['table-column--icon'],
                    )}
                >
                </div>
            </div>
            {
                list.map((it, index) => (
                    <div
                        key={index}
                        className={styles['table-row']}
                    >
                        <div
                            className={classNames(
                                styles['table-column'],
                                styles['table-column--shortcut']
                            )}
                        >
                            <span
                                className={styles['shortcut']}
                            >
                                {it.shortcut}
                            </span>
                        </div>
                        <div
                            className={classNames(
                                styles['table-column'],
                                styles['table-column--rate']
                            )}
                        >
                            <span>{it.rate}</span>
                        </div>
                        <div
                            className={classNames(
                                styles['table-column'],
                                styles['table-column--icon']
                            )}
                        >
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
                    </div>
                ))
            }
        </div>
    );
};

export default CurrencyPairsRatesList;
