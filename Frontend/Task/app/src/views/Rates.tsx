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

    constructor(props: object) {
      super(props);
      this._events.subscribe(this.setCurrencies.bind(this));
    }

    public componentWillMount() {
      
    }

    public setCurrencies(message: EEventMessages, value: any) {
      let state = this.state;
      this.state.display = function() { return this.visibility ? "block" : "none"};

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
        state.downloadError = false;
      }
      if (message == EEventMessages.MenuTabSelected) {
        let data: { tab: string } = value;
        if (data.tab == "rates") {
          state.visibility = true;
        } else {
          state.visibility = false;
        }
      }
      if (message == EEventMessages.RatesRefreshFailed) {
        state.downloadError = true;
      }
      this.setState(state);
    }

    private createCurrencyTableLines() {
      var me = this;
      
      return this.state.currencies.filter(function(x) { 
          return x.enabledInView
        }).map(function(x) { 
          return <div key={x.exchangeCode}>{x.currencyCodeFrom}/{x.currencyCodeTo}, Trend: <me.getTrend currencyPairsModel={x}></me.getTrend> ({x.rate})</div>
        });
    }

    private getTrend(props: { currencyPairsModel: CurrencyPairsModel }) {
      let response;
      if (props.currencyPairsModel.trend() == EExchangeRateTrend.UP) {
        response = <i className="fas fa-long-arrow-alt-up red"></i>;
      } else if (props.currencyPairsModel.trend() == EExchangeRateTrend.DOWN) {
        response = <i className="fas fa-long-arrow-alt-down green"></i>;
      } else if (props.currencyPairsModel.trend() == EExchangeRateTrend.UNKNOWN) {
        response = <i className="fas fa-question red" title="Prozatím nelze zjistit vývoj hodnoty."></i>;
      } else {
        response = <i className="fas fa-long-arrow-alt-right blue"></i>;
      }
      
      return response;
    }

    private informationMessage(props: { errorMessageVisible: boolean }) {
      if (props.errorMessageVisible) {
        return <div className="small red paddingBottom10">Došlo k chybě při načítání, data nemusí odpovídat nejnovějším hodnotě.</div>;
      } else {
        return <div></div>;
      }
    }

    private currencyTable(props: { visibility: boolean, instance: Rates }) {
      var display = props.visibility ? "block" : "none";
      var errorMessageVisible = props.instance.state.downloadError;
      
      return <div className="currencyTable" style={{display: display}}>
        <props.instance.informationMessage errorMessageVisible={errorMessageVisible}></props.instance.informationMessage>
        {props.instance.createCurrencyTableLines()}
      </div>
    }
    
    public render() {
      return (
        <this.currencyTable visibility={this.state.visibility} instance={this}></this.currencyTable>
      );
    }
}
