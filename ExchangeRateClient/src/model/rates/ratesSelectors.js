import {createSelector} from 'reselect';

const selectCurrencyPairs = state => state.configuration.loaded ?
    state.configuration.data.currencyPairs :
    null;

const selectRates = state => state.rates.data;

const selectPreviousRates = state => state.rates.previousData;

const selectFilter = state => state.filter;

export const matchesFilter = (currency, filter) => {
    if (filter === '') {
        return true;
    }

    return currency.code.indexOf(filter) > -1 || currency.name.indexOf(filter) > -1;
};

const compare = (a, b) => {
    if (a > b) {
        return -1;
    }

    if (b > a) {
        return 1;
    }

    return 0;
}

export const selectProcessedRates = createSelector(
    selectCurrencyPairs,
    selectRates,
    selectPreviousRates,
    selectFilter,
    (currencyPairs, rates, previousRates, filter) => {
        if (currencyPairs === null || rates === null) {
            return null;
        }

        const pairIds = Object.keys(currencyPairs);
        const filteredPairIds = pairIds.filter(
            id => matchesFilter(currencyPairs[id][0], filter) ||
                matchesFilter(currencyPairs[id][1], filter)
        );

        return filteredPairIds.map(id => ({
            id,
            fromCurrency: currencyPairs[id][0],
            toCurrency: currencyPairs[id][1],
            rate: rates[id],
            trend: previousRates === null ? null : compare(previousRates[id], rates[id]),
        }));
    }
)
