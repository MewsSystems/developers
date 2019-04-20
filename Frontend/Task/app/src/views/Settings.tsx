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

    public setCurrencies(message: EEventMessages, value: any) {
      var state = this.state;
      state.display = function() { return this.visibility ? "block" : "none"};
      if (message == EEventMessages.ConfigurationLoaded) {
        let data: Array<CurrencyPairsModel> = value;
        let state = new CurrencyPairsState();
        state.display = function() { return this.visibility ? "block" : "none"};

        for (var item of data) {
          let currencyPairsModel = item;
          state.currencies.push(currencyPairsModel);
        }
        this.setState(state);
      }
      if (message == EEventMessages.MenuTabSelected) {
        let data: { tab: string } = value;
        if (data.tab == "settings") {
          state.visibility = true;
        } else {
          state.visibility = false;
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

    private createCurrencyTableLines() {
      var me = this;
      
      return this.state.currencies.map(function(x) { 
         return <div key={x.exchangeCode}><input type="checkbox" defaultChecked={x.enabledInView} onChange={me.handleCheckboxChange.bind(me)} data-exchangecode={x.exchangeCode} /><span title={x.currencyTitle}>{x.currencyCodeFrom}/{x.currencyCodeTo} {x.enabledInView}</span></div>
        });
    }

    private currencyTable(props: { visibility: boolean, instance: Settings }) {
      var display = props.visibility ? "block" : "none";

      return <div className="currencyTable" style={{display: display}}>
        {props.instance.createCurrencyTableLines()}
      </div>
    }
    
    public render() {
      return (
        <this.currencyTable visibility={this.state.visibility} instance={this}></this.currencyTable>
      );
    }
}
