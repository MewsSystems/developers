import { Dispatch } from 'react';
import axios, { AxiosResponse } from 'axios';
import * as R from 'ramda';
import { ExchangeAction } from '../interface/exchangeActionInterface';
import { ExchangeInterface } from '../interface/exchangeInterface';
import { Trend } from '../interface/currencyPairInterface';
import config from '../config';
import {MessageType} from '../interface/MessageInterface';

export const resolveTrend = (rate, newRate) => {
    if (!rate || !newRate) {
        return;
    }

    if (rate > newRate) {
        return Trend.DECLINING;
    } else if (rate < newRate) {
        return Trend.GROWING;
    } else {
        return Trend.STAGNATING;
    }
};

export const mapRate = (pair, key, rates) => {
    const { rate, trend } = pair;
    const newRate = R.has(key, rates) ? rates[key] : rate;
    const newTrend = rate && newRate ? resolveTrend(rate, newRate) : trend;

    return {...pair, trend: newTrend, rate: newRate};
};

export default () => async (dispatch: Dispatch<any>, getState: () => ExchangeInterface) => {
    try {
        const { pairs = {} } = getState();
        const response: AxiosResponse<any> = await axios.get(
            `${config.endpoint}/rates`,
            { params: {  currencyPairIds: Object.keys(pairs) }},
        );
        const { data = {} } = response;
        const { rates = {}} = data;

        dispatch({
            type: ExchangeAction.GET_PAIRS,
            payload: R.mapObjIndexed((pair, key) => mapRate(pair, key, rates), pairs),
        });
        dispatch({
            type: ExchangeAction.ADD_MESSAGE,
            payload: {},
        });
    } catch (error) {
        dispatch({
            type: ExchangeAction.ADD_MESSAGE,
            payload: {
                message: 'Exchange rates are not up-to-date. Please wait few seconds to refresh.',
                type: MessageType.ERROR,
            },
        });
    }
};
