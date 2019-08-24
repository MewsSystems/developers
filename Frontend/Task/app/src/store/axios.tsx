import axios from 'axios';
import { endpoint, interval } from '../config.json';

const axiosInterceptor = axios.create({
  baseURL: endpoint,
  timeout: interval
});

export default axiosInterceptor;