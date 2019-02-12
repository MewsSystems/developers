import React, {Component} from 'react'; //je potřeba aby fungovala 'jsx' syntaxe
import { mainColor } from '../config';
import ReactLoading from 'react-loading';
import CurrencyPairCheckbox from './currencyPairCheckbox';

class CurrencyPairsSelector extends Component {

    render() {
        var {configurationIds} = this.props;
        if(!configurationIds) {
            return <ReactLoading color={mainColor} type="bars"/>
        }
        else {
            return (
            <div>
                <h1>Choose currency pairs</h1>
                <ul>
                    {Object.keys(configurationIds).map(function(keyName, KeyIndex){
                        return <CurrencyPairCheckbox 
                            currencyPair={configurationIds[keyName]} 
                            key={keyName} 
                            keyName={keyName} 
                            addRemoveCurrencyPair={(pairId) => this.props.addRemoveCurrencyPair(pairId)
                        }/>
                    },this)}
                </ul>
            </div>); 
        }        
    }
}


export default CurrencyPairsSelector;