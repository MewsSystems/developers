import React, {Component} from 'react'; //je potřeba aby fungovala 'jsx' syntaxe

class CurrencyPairCheckbox extends Component {
    
    render() {
        var {currencyPair, keyName, addRemoveCurrencyPair} = this.props;
        return (
            <div className="checkBox">
                <input type="checkbox" id={keyName} className="" onChange={(pairId) => addRemoveCurrencyPair(keyName)} />            
                <label htmlFor={keyName}>{currencyPair[0].code + ' / ' + currencyPair[1].code}</label>
            </div>
        );
    }
}
export default CurrencyPairCheckbox;