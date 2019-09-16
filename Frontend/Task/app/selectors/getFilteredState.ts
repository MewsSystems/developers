import { createSelector } from 'reselect'
import { ApplicationState, CurrencyState } from '@store/types';
import { RatesTableProps } from '@components/RatesTable/types';

const currencyPairs = (currencyState: CurrencyState) => (currencyState.currencyPairs)
const urlParams = (currencyState: CurrencyState, props: RatesTableProps) => (props.urlParams)
const currencyPairsIds = (state: CurrencyState) => (state.currencyPairsIds)

export const getFilteredState = createSelector(
    [currencyPairs, urlParams, currencyPairsIds],
    (currencyPairs, urlParams, currencyPairsIds) => {
        return currencyPairsIds.filter(id =>
            currencyPairs[id][0].code.includes(urlParams.searchTerm) || currencyPairs[id][1].code.includes(urlParams.searchTerm)
        )
    }
);