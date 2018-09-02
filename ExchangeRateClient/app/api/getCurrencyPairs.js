import axios from 'axios';
import { urls } from '../constants';

export default () => axios.get(urls.CURRENCY_PAIRS);
