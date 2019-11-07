import React, { useEffect, useState } from 'react';
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
        return Object.keys(data).map(currencyPairId => {
            return {
                id: currencyPairId,
                code: `${data[currencyPairId][0].code} / ${data[currencyPairId][1].code}`,
                name: `${data[currencyPairId][0].name} / ${data[currencyPairId][1].name}`,
            };
        });
    } else {
        return [];
    }
};

const getCurrencyPairsSelectorOptions = (currencyPairsMap = []) => {
    const options = [];
    if (!isEmpty(currencyPairsMap)) {
        for (let currencyPair of currencyPairsMap) {
            options.push({
                value: currencyPair.id,
                label: `${currencyPair.code} - ${currencyPair.name}`
            });
        }
    }
    return options;
};

const getCurrencyPairsRatesList = (currencyPairsMap = [], prevRates = {}, rates = {}, selectedCurrencyPairsIds = []) => {
    const list = [];
    if (!isEmpty(currencyPairsMap) && !isEmpty(rates)) {
        for (let currencyPair of currencyPairsMap) {
            list.push({
                ...currencyPair,
                rate: rates[currencyPair.id],
                prevRate: isEmpty(prevRates) ? undefined : prevRates[currencyPair.id],
                selected: isEmpty(selectedCurrencyPairsIds) ? true : selectedCurrencyPairsIds.includes(currencyPair.id)
            });
        }
    }
    return list;
};


const App = ({
                 fetchCurrencyPairs,
                 fetchCurrencyPairsRates,
                 currencyPairs = {data: {}, loading: false, error: null},
                 currencyPairsRates = {data: {}, loading: false, error: null},
                 selectedCurrencyPairsIds = []
             }) => {

    const [currencyPairsMap, setCurrencyPairsMap] = useState([]);

    const [
        selectedCurrencyPairsIdsInitialValue,
        setSelectedCurrencyPairsIdsInitialValue,
    ] = useSessionStorage('selectedCurrencyPairsIds');

    const prevRates = usePrevious(currencyPairsRates.data);
    const currencyPairsRatesList = getCurrencyPairsRatesList(currencyPairsMap, prevRates, currencyPairsRates.data, selectedCurrencyPairsIds);
    const currencyPairsSelectorOptions = getCurrencyPairsSelectorOptions(currencyPairsMap);

    useEffect(() => {
        fetchCurrencyPairs();
        // eslint-disable-next-line
    }, []);

    useEffect(() => {
        if (isEmpty(currencyPairsMap)) {
            setCurrencyPairsMap(getCurrencyPairsMap(currencyPairs.data));
        }
        // eslint-disable-next-line
    }, [currencyPairs]);

    useEffect(() => {
        setSelectedCurrencyPairsIdsInitialValue(selectedCurrencyPairsIds);
        // eslint-disable-next-line
    }, [selectedCurrencyPairsIds]);


    useInterval(() => {
        if (!isEmpty(currencyPairs.data)) {
            fetchCurrencyPairsRates(Object.keys(currencyPairs.data));
        }
    }, 2000);

    return (
        <LoadingOverlay
            active={isEmpty(currencyPairsSelectorOptions) || isEmpty(currencyPairsRatesList)}
            spinner
            text='Loading App...'
        >
            <div
                className={styles['content']}
            >
                <CurrencyPairsSelector
                    options={currencyPairsSelectorOptions}
                    loading={currencyPairs.loading}
                    error={currencyPairs.error}
                    initialValues={{
                        selectedCurrencyPairsIds: selectedCurrencyPairsIdsInitialValue || []
                    }}
                />
                <CurrencyPairsRatesList
                    currencyPairsRatesList={currencyPairsRatesList}
                    error={currencyPairsRates.error}
                />
            </div>
        </LoadingOverlay>
    );
};

const selector = formValueSelector('currencyPairsSelector');

const mapStateToProps = state => ({
    currencyPairs: selectCurrencyPairs(state),
    currencyPairsRates: selectCurrencyPairsRates(state),
    selectedCurrencyPairsIds: selector(state, 'selectedCurrencyPairsIds')
});

const mapDispatchToProps = {
    fetchCurrencyPairs,
    fetchCurrencyPairsRates,
};

export default connect(mapStateToProps, mapDispatchToProps)(App);
