import config from '../../config/config.json'
import {currencyPairs} from "../../interfaces/currencyPairs";
import {rate} from "../../interfaces/rate";
import {forIn} from 'lodash'
import axios from 'axios'
import store from '../store'

export enum currencyPairsRatesActionsType {
    GET_CONFIG_SUCCESS = 'GET_CONFIG_SUCCESS',
    GET_CONFIG_LOADING = 'GET_CONFIG_LOADING',
    GET_CONFIG_ERROR = 'GET_CONFIG_ERROR',
    GET_RATES_SUCCESS = 'GET_RATES_SUCCESS',
    GET_RATES_LOADING = 'GET_RATES_LOADING',
    GET_RATES_ERROR = 'GET_RATES_ERROR',
    UPDATE_FILTERS = 'UPDATE_FILTERS'
}

export function get_config() {
    return (dispatch: any) => getData(dispatch)
}

export function get_config_success(currencyPairs: currencyPairs) {
    return {
        type: currencyPairsRatesActionsType.GET_CONFIG_SUCCESS,
        currencyPairs
    }
}

export function get_config_loading(configLoading: boolean) {
    return {
        type: currencyPairsRatesActionsType.GET_CONFIG_LOADING,
        configLoading
    }
}

export function get_config_error(configError: boolean) {
    return {
        type: currencyPairsRatesActionsType.GET_CONFIG_ERROR,
        configError
    }
}

export function get_rates(ids: string[]) {
    return (dispatch: any) => getRates(dispatch, ids)
}

export function get_rates_success(rates: rate) {
    return {
        type: currencyPairsRatesActionsType.GET_RATES_SUCCESS,
        rates
    }
}

export function get_rates_loading(ratesLoading: boolean) {
    return {
        type: currencyPairsRatesActionsType.GET_RATES_LOADING,
        ratesLoading
    }
}

export function get_rates_error(ratesError: boolean) {
    return {
        type: currencyPairsRatesActionsType.GET_RATES_ERROR,
        ratesError
    }
}

export function update_filters(filter: string) {

    let filters: string[] = store.getState().filters;
    if (filters.includes(filter))
        filters.splice(filters.indexOf(filter), 1);
    else
        filters.push(filter);
    localStorage.setItem('filters', JSON.stringify(filters));
    return {
        type: currencyPairsRatesActionsType.UPDATE_FILTERS,
        filters
    }
}

function getData(dispatch: any) {
    const url: string = config.init;
    const cached = localStorage.getItem('config');
    let data: currencyPairs;
    dispatch(get_config_loading(true));
    if (cached) {
        dispatch(get_rates(Object.keys(JSON.parse(cached))));
        dispatch(get_config_success(JSON.parse(cached)));
        dispatch(get_config_loading(false));
    } else {
        axios.get(url)
            .then(config => {
                data = config.data.currencyPairs;
                dispatch(get_config_loading(false));
                dispatch(get_config_success(data));
                localStorage.setItem('config', JSON.stringify(data));
            })
            .then(() => {
                dispatch(get_rates(Object.keys(data)))
            })
            .catch((error) => {
                console.log(error);
                dispatch(get_config_error(true));
            });
    }
}

function getRates(dispatch: any, ids: string[]) {
    let url = config.rates;
    ids.forEach(item => url += `currencyPairIds=${item}&`);
    fetchRates(dispatch, url);
    setInterval(() => {
        fetchRates(dispatch, url)
    }, config.interval);
}

function fetchRates(dispatch: any, url: string) {
    dispatch(get_rates_loading(true));
    axios.get(url)
        .then(rates => {
            const data: rate = rates.data.rates;
            forIn(data, (value, key) => {
                let trend = 'stagnating';
                if (store.getState().rates[key]) {
                    if (data[key] > store.getState().rates[key].value)
                        trend = 'growing';
                    if (data[key] < store.getState().rates[key].value)
                        trend = 'falling';
                    if (data[key] === store.getState().rates[key].value)
                        trend = 'stagnating';
                }
                data[key] = {value: value, trend};
            });
            dispatch(get_rates_success(data));
            dispatch(get_rates_loading(false));
        })
        .catch((error) => {
            console.log(error);
            dispatch(get_rates_error(true));
        });
}

