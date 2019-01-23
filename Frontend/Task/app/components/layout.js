import React from 'react'; //je potřeba aby fungovala 'jsx' syntaxe
import ReactDOM from 'react-dom'; //vstupní bod pro renderování react
import CurrencyPairsSelector from './currency-paris-selector';
import CurrencyPairsRate from './currency-pair-rate-list';

class AppLayout extends React.Component {

    
    render() {
        return <div>
            <CurrencyPairsSelector />
            <CurrencyPairsRate />
        </div>;
    }
}
export default AppLayout;