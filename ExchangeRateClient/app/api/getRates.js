import axios from 'axios';
import { urls } from '../constants';
import { arrayToGetParams } from './utils';

export default (currencyPairIds) =>
    axios.get(`${urls.RATES}?${arrayToGetParams('currencyPairIds')(currencyPairIds)}`);
