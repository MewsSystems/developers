import React, { Component } from 'react';
import EventAggregator from '../EventAggregator';
import CurrencyPairsState from '../models/CurrencyPairsState';
import CurrencyPairsModel from '../models/CurrencyPairsModel';
import EEventMessages = require('../abstracts/EEventMessages');

export class Settings extends Component {
    public state: CurrencyPairsState = new CurrencyPairsState();
    private _events: EventAggregator = EventAggregator.instance as EventAggregator;

    constructor(props: object) {
      super(props);
      this._events.subscribe(this.setCurrencies.bind(this));
    }

    public componentWillMount() {

    }

    public setCurrencies(message: EEventMessages, value: Array<CurrencyPairsModel>) {
        if (message == EEventMessages.ConfigurationLoaded) {
          let state = new CurrencyPairsState();
          for (var item of value) {
            let currencyPairsModel = item;
            state.currencies.push(currencyPairsModel);
          }
          this.setState(state);
        }
    }

    private handleCheckboxChange(e: any) {
      // send event
      this._events.notify(EEventMessages.CurrencyDisplayChanged, {
        exchangeCode: e.target.getAttribute("data-exchangecode"),
        newState: e.target.checked
      })
    }

    private createCurrencyTable() {
      var me = this;
      
      return this.state.currencies.map(function(x) { 
         return <div key={x.exchangeCode}><input type="checkbox" defaultChecked={x.enabledInView} onChange={me.handleCheckboxChange.bind(me)} data-exchangecode={x.exchangeCode} /><span title={x.currencyTitle}>{x.currencyCodeFrom}/{x.currencyCodeTo} {x.enabledInView}</span></div>
        });
    }
    
    public render() {
        return (
          <div className="CurrencyTable">
            {this.createCurrencyTable()}
          </div>
        );
    }
}
