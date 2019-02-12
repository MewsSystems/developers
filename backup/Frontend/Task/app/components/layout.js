import React, {Component} from 'react'; //je potřeba aby fungovala 'jsx' syntax
import CurrencyPairsSelector from './currencyPairsSelector';
import CurrencyPairsRate from './currencyPairRateList';
import { server } from '../config';

class AppLayout extends Component {

    state = {
        currencyPairIds: [],
        configurationIds: null
    }

    render() {        
        return (
        <div>
            <CurrencyPairsSelector configurationIds={this.state.configurationIds} addRemoveCurrencyPair={(pairId) => this.addRemoveCurrencyPair(pairId)}/>
            <CurrencyPairsRate configurationIds={this.state.configurationIds} currencyPairIds={this.state.currencyPairIds}/>
        </div>); 
    }

    addRemoveCurrencyPair(pairId) {
        var currencyPairIds = this.state.currencyPairIds;
        if(currencyPairIds.includes(pairId)){
            //remove
            var index = currencyPairIds.indexOf(pairId); 
            currencyPairIds.splice(index, 1);
        }
        else{
            //add
            currencyPairIds.push(pairId);
        }
        this.setState({currencyPairIds: currencyPairIds});
    }

    componentDidMount() {
        fetch(`${server}/configuration`)
        .then(res => res.json())
        .then(json => {
            this.setState({
                configurationIds: json["currencyPairs"],
            });
        })
    }
} 

export default AppLayout;