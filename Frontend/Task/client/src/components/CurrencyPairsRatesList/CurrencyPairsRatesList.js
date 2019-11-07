import React from 'react';
import { isEmpty, isNil } from 'ramda';
import styles from './CurrencyPairsRatesList.module.css';
import classNames from 'classnames';
import LoadingOverlay from 'react-loading-overlay';

const CurrencyPairsRatesList = ({currencyPairsRatesList = [], error = null}) => {

    const renderIcon = (prevRate, rate) => {
        if (isNil(prevRate) || isNil(rate)) {
            return null;
        } else if (prevRate < rate) {
            return <span>&#x2191;</span>;
        } else if (prevRate < rate) {
            return <span>&#x2193;</span>;
        } else {
            return <span>&#x3d;</span>;
        }
    };

    return isEmpty(currencyPairsRatesList)
        ? (
            <div
                style={{textAlign: 'center'}}
            >Loading rates...
            </div>
        )
        : (
            <div
                className={styles['table']}
            >
                <LoadingOverlay
                    active={!isNil(error)}
                    text='Reconnecting to server...'
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
                        currencyPairsRatesList.map((it, index) => {

                            return !it.selected ? null : (
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
                                {it.code}
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
                                        {renderIcon(it.prevRate, it.rate)}
                                    </div>
                                </div>
                            );
                        })
                    }
                </LoadingOverlay>
            </div>
        );
};

export default CurrencyPairsRatesList;

