import { Rate, StringTMap, CurrencyPair } from "@store/types";
import { Actions as CurrencyPairsActions } from '@store/reducers/currencyPairs.reducer';

export interface PropsFromState {
    loading: boolean,
    currencyPairs:  StringTMap<CurrencyPair>,
    currencyPairsIds: string[],
    rates: StringTMap<Rate>,
}

export interface PropsFromDispatch {
    fetchCurrencyPairs: typeof CurrencyPairsActions.fetchCurrencyPairs
}

export type ExchangeRatesProps = PropsFromState & PropsFromDispatch;