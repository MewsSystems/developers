import { createSelector } from 'reselect'
import { ApplicationState, KeyByCurrencyPair } from '../store/types';
import { getTrend } from '../utils/getTrend';
import { ExchangeRatesProps } from '../containers/types';

/*
// this will make faster the calculation of normalize funtion
// as we have many items and to avoid the multiple creation of new Date()
const today = new Date();

const currencyPairsSelector = (state: ConfigurationState) => (state.configuration.currencyPairs)
const ratesSelector = (state: ConfigurationState) => (state.configuration.rates)

export const createCurrencyPairList = createSelector(
    [currencyPairsSelector],
    pairs => pairs.map((pair: KeyByCurrencyPair) => ({
        ...pair,

    }))
)

const currencyPairs = (state: ApplicationState) => (state.currencyPairs)
const rates = (state: ApplicationState) => (state.rates)
const currencyPairsRates = (state: ApplicationState) => (state.currencyPairsRates)

export const createCurrencyPairsRates = createSelector(
    [currencyPairs, rates, currencyPairsRates],
    (currencyPairs, rates, currencyPairsRates) => {
        let temp: any = {};

        if(currencyPairs == undefined || rates == undefined) {
            return temp;
        }

        for (let id of Object.keys(rates)) {
            const rate = rates[id as any];

            temp[id] = {
                id: id,
                code: `${currencyPairs[id as any][0].code}/${currencyPairs[id as any][0].code}`,
                trend: getTrend(currencyPairsRates && currencyPairsRates[id] && currencyPairsRates[id].rate || 0, rate),
                rate: rates[id as any]
            }
        }
      return temp;
    }
  )
  */