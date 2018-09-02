import moment from 'moment';
import { append, mapObjIndexed, prop } from 'ramda';
import { isNilOrEmpty } from 'ramda-adjunct';
import { getRatesHistory, getRateHistoryEntries } from '../../selectors';

const createNewEntry = (rate) => ({ timestamp: moment().format('MMMM Do YYYY, h:mm:ss a'), rate });

export default (state) => (responseData) => {
    const currentRatesHistory = getRatesHistory(state);
    const newRates = prop('rates', responseData);
    return mapObjIndexed((rateHistory, currencyPairId) => {
        const currentEntries = getRateHistoryEntries(currencyPairId)(state);
        const newRate = prop(currencyPairId, newRates);
        return isNilOrEmpty(newRate) ? currentEntries : append(createNewEntry(newRate), currentEntries);
    }, currentRatesHistory);
};
