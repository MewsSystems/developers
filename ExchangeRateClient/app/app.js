import { endpoint, interval } from './config';
import startRatePolling from './startRatePolling';

export function run() {
    console.log('App is running.');
    startRatePolling(endpoint, interval);
}
