import React, { useEffect } from 'react';
import { isEmpty } from 'ramda';
import { connect } from 'react-redux';
import { formValueSelector } from 'redux-form';
import CurrencyPairsSelector from '../CurrencyPairsSelector';
import CurrencyPairsRatesList from '../CurrencyPairsRatesList';
import { fetchCurrencyPairs, selectCurrencyPairs } from '../../redux/ducks/currencyPairs';
import { fetchCurrencyPairsRates, selectCurrencyPairsRates } from '../../redux/ducks/currencyPairsRates';
import { useInterval } from '../../hooks';

const selector = formValueSelector('currencyPairsSelector');


const App = ({
                 fetchCurrencyPairs,
                 fetchCurrencyPairsRates,
                 currencyPairs = {data: {}},
                 currencyPairsRates = {data: {}, error: null},
                 selectedCurrencyPairsIds = []
             }) => {

    useInterval(() => {
        if (!isEmpty(currencyPairs.data)) {
            if (isEmpty(selectedCurrencyPairsIds)) {
                fetchCurrencyPairsRates(Object.keys(currencyPairs.data));
            } else {
                fetchCurrencyPairsRates(selectedCurrencyPairsIds);
            }
        }
    }, 2000);

    useEffect(() => {
        fetchCurrencyPairs();
    }, [fetchCurrencyPairs]);

    return isEmpty(currencyPairs.data)
        ? (
            <div>
                Loading configuration...
            </div>
        )
        : (
            <div>
                <CurrencyPairsSelector
                    currencyPairs={currencyPairs.data}
                />
                {
                    isEmpty(currencyPairsRates.data)
                        ? (
                            <div>
                                Loading rates...
                            </div>
                        )
                        : (
                            <CurrencyPairsRatesList
                                currencyPairs={currencyPairs.data}
                                currencyPairsRates={currencyPairsRates.data}
                            />
                        )
                }
            </div>
        );
};

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
