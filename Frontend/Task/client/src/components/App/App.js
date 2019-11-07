import React, { useEffect, useMemo } from 'react';
import { isEmpty } from 'ramda';
import { connect } from 'react-redux';
import { formValueSelector } from 'redux-form';
import CurrencyPairsSelector from '../CurrencyPairsSelector';
import { fetchCurrencyPairs, selectCurrencyPairs } from '../../redux/ducks/currencyPairs';
import { fetchCurrencyPairsRates, selectCurrencyPairsRates } from '../../redux/ducks/currencyPairsRates';
import { useInterval, usePrevious, useSessionStorage } from '../../hooks';
import styles from './App.module.css';
import CurrencyPairsRatesList from '../CurrencyPairsRatesList';
import LoadingOverlay from 'react-loading-overlay';

const getCurrencyPairsMap = (data = {}) => {
    if (!isEmpty(data)) {
        const map = {};
        for (let currencyPairId in data) {
            map[currencyPairId] = {
                code: `${data[currencyPairId][0].code} / ${data[currencyPairId][1].code}`,
                name: `${data[currencyPairId][0].name} / ${data[currencyPairId][1].name}`,
            };
        }
        return map;
    } else {
        return {};
    }
};

const getCurrencyPairsSelectorOptions = (currencyPairsMap = {}) => {
    const options = [];
    if (!isEmpty(currencyPairsMap)) {
        for (let currencyPairId in currencyPairsMap) {
            options.push({
                value: currencyPairId,
                label: `${currencyPairsMap[currencyPairId].code} - ${currencyPairsMap[currencyPairId].name}`
            });
        }
    }
    return options;
};

const getCurrencyPairsRatesList = ({
                                       currencyPairsMap = {},
                                       rates = {},
                                       prevRates = {},
                                       selectedCurrencyPairsIds = [],
                                   }) => {
    const list = [];
    if (!isEmpty(currencyPairsMap) && !isEmpty(rates)) {
        for (let currencyPairId in currencyPairsMap) {
            list.push({
                ...currencyPairsMap[currencyPairId],
                rate: rates[currencyPairId],
                prevRate: isEmpty(prevRates) ? undefined : prevRates[currencyPairId],
                selected: isEmpty(selectedCurrencyPairsIds) ? true : selectedCurrencyPairsIds.includes(currencyPairId)
            });
        }
    }
    return list;
};


const App = ({
                 fetchCurrencyPairs,
                 fetchCurrencyPairsRates,
                 configuration = {data: {}, loading: false, error: null},
                 currencyPairsRates = {data: {}, loading: false, error: null},
                 selectedCurrencyPairsIds = []
             }) => {

    const [cachedConfiguration, setCachedConfiguration] = useSessionStorage('configuration', {});
    const [cachedSelectedCurrencyPairsIds, setCachedSelectedCurrencyPairsIds] = useSessionStorage('selectedCurrencyPairsIds', []);
    const currencyPairsMap = useMemo(() => getCurrencyPairsMap(cachedConfiguration), [cachedConfiguration]);
    const currencyPairsSelectorOptions = useMemo(() => getCurrencyPairsSelectorOptions(currencyPairsMap), [currencyPairsMap]);
    const prevRates = usePrevious(currencyPairsRates.data);
    const currencyPairsRatesList = getCurrencyPairsRatesList({
        currencyPairsMap,
        rates: currencyPairsRates.data,
        prevRates: prevRates,
        selectedCurrencyPairsIds: cachedSelectedCurrencyPairsIds,
    });

    // Fetch Configuration
    useEffect(() => {
        if (isEmpty(cachedConfiguration)) {
            fetchCurrencyPairs();
        }
        // eslint-disable-next-line
    }, []);

    // Cache Configuration
    useEffect(() => {
        if (!isEmpty(configuration.data)) {
            setCachedConfiguration(configuration.data);
        }
        // eslint-disable-next-line
    }, [configuration.data]);

    // Cache user selectedCurrencyPairsIds
    useEffect(() => {
        setCachedSelectedCurrencyPairsIds(selectedCurrencyPairsIds);
        // eslint-disable-next-line
    }, [selectedCurrencyPairsIds]);

    useInterval(() => {
        if (!isEmpty(currencyPairsMap)) {
            fetchCurrencyPairsRates(Object.keys(currencyPairsMap));
        }
    }, 2000);

    return (
        <LoadingOverlay
            active={isEmpty(currencyPairsSelectorOptions)}
            spinner
            text='Loading Configuration...'
        >
            <div
                className={styles['content']}
            >
                <CurrencyPairsSelector
                    options={currencyPairsSelectorOptions}
                    loading={configuration.loading}
                    error={configuration.error}
                    initialValues={{
                        selectedCurrencyPairsIds: cachedSelectedCurrencyPairsIds,
                    }}
                />
                {
                    !isEmpty(currencyPairsSelectorOptions) &&
                    <CurrencyPairsRatesList
                        currencyPairsRatesList={currencyPairsRatesList}
                        error={currencyPairsRates.error}
                    />
                }
            </div>
        </LoadingOverlay>
    );
};

const selector = formValueSelector('currencyPairsSelector');

const mapStateToProps = state => ({
    configuration: selectCurrencyPairs(state),
    currencyPairsRates: selectCurrencyPairsRates(state),
    selectedCurrencyPairsIds: selector(state, 'selectedCurrencyPairsIds')
});

const mapDispatchToProps = {
    fetchCurrencyPairs,
    fetchCurrencyPairsRates,
};

export default connect(mapStateToProps, mapDispatchToProps)(App);
