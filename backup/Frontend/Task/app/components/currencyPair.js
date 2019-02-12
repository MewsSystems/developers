import React, {Component} from 'react'; //je potřeba aby fungovala 'jsx' syntaxe
import ReactLoading from 'react-loading';

class CurrencyPair extends Component {

    render(){
        var {currencyPair, newRate, oldRate} = this.props;
        if(currencyPair != undefined) {
            
            return (
                <tr>
                    <td>{currencyPair[0].name + ' / ' + currencyPair[1].name}</td>
                    <td>{newRate ? newRate : '-'}</td>
                    <td>{this.getTrend(newRate, oldRate)}</td>
                </tr>
            );
        }
        return (
            <tr><td colSpan="3"><ReactLoading color="white" type="bars"/></td></tr>
        );
    }

    getTrend(newRate, oldRate){
        if(oldRate != null){
            if(newRate > oldRate){
                return "growing";
            }
            else if(newRate < oldRate){
                return "declining";
            }
            return "stagnating"; 
        }
        return (
            <ReactLoading color="white" type="cylon"/>
        );        
    }
}
export default CurrencyPair;