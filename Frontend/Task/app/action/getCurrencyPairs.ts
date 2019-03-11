import { Dispatch } from 'react';
import axios from 'axios';
import * as R from 'ramda';
import { ExchangeAction } from '../interface/exchangeActionInterface';
import { CurrencyInterface } from '../interface/currencyInterface';
import { getPairCollectionWithShortcut } from './currencyPair';
import getRates from './getRates';
import config from '../config';

export default () => async (dispatch: Dispatch<any>) => {
    try {
        dispatch({type: ExchangeAction.LOADING_TOGGLE});
        const response = await axios.get(`${config.endpoint}/configuration`);
        const { data = {} } = response;
        const { currencyPairs = {} } = data;
        const pairs = getPairCollectionWithShortcut(
            R.map((pair: CurrencyInterface[]) => ({ pair }), currencyPairs),
        );

        dispatch({
            type: ExchangeAction.GET_PAIRS,
            payload: pairs,
        });
        dispatch(getRates());
        dispatch({type: ExchangeAction.LOADING_TOGGLE});
    } catch (error) {
        dispatch({type: ExchangeAction.LOADING_TOGGLE});
        dispatch({type: ExchangeAction.ERROR_TOGGLE});
    }
};
