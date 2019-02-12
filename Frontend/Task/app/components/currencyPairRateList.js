import React, {Component} from 'react'; //je potřeba aby fungovala 'jsx' syntaxe
import { endpoint, interval } from '../config';
import CurrencyPair from './currencyPair';
import Table from 'react-bootstrap/Table';
import Card from 'react-bootstrap/Card';


class CurrencyPairsRate extends Component {

    state = {
        newRates: [],
        oldRates: []
    }
    componentWillUnmount() {
        clearInterval(this.intervalRates);
    }
//jen jednou
    componentDidMount() {

        var {currencyPairIds} = this.props;
        this.intervalRates = setInterval(() => {
            console.log("Timer ticks..")
            if(currencyPairIds.length > 0){
                var params = this.convertToUrl(currencyPairIds);
                fetch(`${endpoint}?${params}`)
                .then(res => res.json())
                .then(json => {  
                    var oldRates =  this.state.newRates;
                    this.setState({oldRates: oldRates, newRates: json.rates});
                }).catch(function() {
                    console.log("server error");
                });
            }
            
        }, interval);
    }

    convertToUrl(currencyPairIds){
        
        var parameters = [];
        for (var property in currencyPairIds) {
            if (currencyPairIds.hasOwnProperty(property)) {
                parameters.push(encodeURI('currencyPairIds[]=' + currencyPairIds[property]));
            }   
        }
        return parameters.join('&');
    }

    render() {
        var {currencyPairIds, configurationIds} = this.props;
        if(currencyPairIds.length > 0){
            return (
                <div>
                    <Table striped bordered hover variant="dark">
                        <thead>
                            <tr>
                                <th>Currency pair</th>
                                <th>Current value</th>
                                <th>Trend</th>
                            </tr>
                        </thead>
                        <tbody>
                            {Object.keys(currencyPairIds).map(function(keyName, keyIndex){
                                var pairId = currencyPairIds[keyName];
                                return(
                                    <CurrencyPair key={pairId}
                                    currencyPair={configurationIds[pairId]}
                                    newRate={this.state.newRates[pairId]}
                                    oldRate={this.state.oldRates[pairId]}
                                    />
                                );                     
                            }, this)}
                        </tbody>
                    </Table>
                </div>);
        }
        else{
            return (
                <Card bg="warning" text="white">
                    <Card.Body>Please, choose some currency</Card.Body>
                </Card>
            );
        }
    }
}
export default CurrencyPairsRate;