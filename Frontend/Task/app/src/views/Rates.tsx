import React, { Component } from 'react';
import EventAggregator from '../EventAggregator';
import CurrencyPairsState from '../models/CurrencyPairsState';
import CurrencyPairsModel from '../models/CurrencyPairsModel';
import EEventMessages from '../abstracts/EEventMessages';
import ICurrencyDisplayChanged from '../abstracts/ICurrencyDisplayChanged';
import IRatesResponse from '../abstracts/IRatesResponse';
import EExchangeRateTrend from '../abstracts/EExchangeRateTrend';

export class Rates extends Component {
    public state: CurrencyPairsState = new CurrencyPairsState();
    private _events: EventAggregator = EventAggregator.instance as EventAggregator;
    private _timerHandler: number | null = null;

    constructor(props: object) {
      super(props);
      this._events.subscribe(this.setCurrencies.bind(this));
    }

    public componentWillMount() {
      
    }

    public setCurrencies(message: EEventMessages, value: any) {
      let state = this.state;
      if (message == EEventMessages.ConfigurationLoaded) {
        let data: Array<CurrencyPairsModel> = value;
        for (let item of data) {
          let currencyPairsModel = item;
          state.currencies.push(currencyPairsModel);
        }
      }
      if (message == EEventMessages.CurrencyDisplayChanged) {
        let data: ICurrencyDisplayChanged = value;
        for (let item in state.currencies) {
          if (state.currencies[item].exchangeCode == data.exchangeCode) {
            state.currencies[item].enabledInView = data.newState;
          }
        }
      }
      if (message == EEventMessages.RatesRefreshed) {
        let data: IRatesResponse = value;
        for (let item in state.currencies) {
          let code = state.currencies[item].exchangeCode;
          state.currencies[item].rate = data.rates[code] || 0;
        }
      }

      this.setState(state);
    }

    private createCurrencyTable() {
      var me = this;
      
      return this.state.currencies.filter(function(x) { 
          return x.enabledInView
        }).map(function(x) { 
          return <div key={x.exchangeCode}>{x.currencyCodeFrom}/{x.currencyCodeTo}, Trend: {me.getTrend(x)} ({x.rate})</div>
        });
    }

    private getTrend(data: CurrencyPairsModel) {
      let response;
      if (data.trend() == EExchangeRateTrend.UP) {
        response = <i className="fas fa-long-arrow-alt-up red"></i>;
      } else if (data.trend() == EExchangeRateTrend.DOWN) {
        response = <i className="fas fa-long-arrow-alt-down green"></i>;
      } else {
        response = <i className="fas fa-long-arrow-alt-right blue"></i>;
      }
      
      return response;
    }
    
    public render() {
        return (
          <div className="CurrencyTable">
            {this.createCurrencyTable()}
          </div>
        );
    }
}
