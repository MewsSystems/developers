import axios from 'axios';
import { endpoint, interval } from '../config';

const axiosInterceptor = axios.create({
    baseURL: endpoint,
    timeout: interval
});
export default axiosInterceptor;