import { Actions as CurrencyPairsActions } from '@store/reducers/currency-pairs.reducer';
import { RouteComponentProps } from "react-router-dom";

export interface PropsFromState {
    loading: boolean
}

export interface PropsFromDispatch {
    fetchCurrencyPairs: typeof CurrencyPairsActions.fetchCurrencyPairs
}

export type ExchangeRatesProps = PropsFromState & PropsFromDispatch & RouteComponentProps<{}>;


export interface UrlParams {
    searchTerm: string
}