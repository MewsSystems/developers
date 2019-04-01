import { endpoint, interval } from './config';
import { CurrencyPairs } from './src/CurrencyPairs';

export function run(element) {
    console.log('App is running.');

    //ReactDOM.render(React.createElement(CurrencyPairs, {endpoint: endpoint}), document.getElementById('exchange-rate-client'));
    
    var currencyPairs = new CurrencyPairs(element);
}
