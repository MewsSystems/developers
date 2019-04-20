import { endpoint, interval } from '../config.json';
import Singleton from './abstracts/Singleton';
import EventAggregator from './EventAggregator';
import EEventMessages from './abstracts/EEventMessages';
import React from 'react';
import ReactDOM from 'react-dom';
import { Settings } from './views/Settings';
import { Rates } from './views/Rates';
import ApiCall from './modules/ApiCall.js';
import CurrencyPairsModel from './models/CurrencyPairsModel.js';
import ICurrencyPairsResponse from './abstracts/ICurrencyPairsResponse';
import ICurrencyDisplayChanged from './abstracts/ICurrencyDisplayChanged';
import IRatesResponse from './abstracts/IRatesResponse.js';
import { Menu } from './views/Menu.jsx';
import AppStore from './modules/AppStore.js';

class AppCore extends Singleton {
    private _endpointUrl: string;
    private _events: EventAggregator = <EventAggregator>EventAggregator.instance;
    private _store: AppStore = <AppStore>AppStore.instance;
    private _currencyPairsModel: Array<CurrencyPairsModel> = [];

    constructor() {
        super();
        this._endpointUrl = endpoint.replace("/rates", "");
        this._events.subscribe(this.notifyAction.bind(this));
    }

    public get endpointUrl(): string {
        return this._endpointUrl;
    }

    public notifyAction(message: EEventMessages, value: any) {
        if (message == EEventMessages.CurrencyDisplayChanged) {
            let data = value as ICurrencyDisplayChanged;
            if (data.newState) {
                this._store.currencyAdd(data.exchangeCode);
            } else {
                this._store.currencyRemove(data.exchangeCode);
            }
        }
    }

    public async render() {
        ReactDOM.render(
            React.createElement(Menu, {}, null),
            document.getElementById('exchange-rate-client')
        );

        ReactDOM.render(
            React.createElement(Settings, {}, null),
            document.getElementById('root')
        );
        
        ReactDOM.render(
            React.createElement(Rates, {}, null),
            document.getElementById('root-rates')
        );

        await this.getConfiguration();        
        setInterval(this.updateRates.bind(this), interval);
        this.updateRates();
        
        let tab = this._store.getTab();
        this._events.notify(EEventMessages.MenuTabSelected, { tab: tab });
    }

    private async getConfiguration() {
        let api = new ApiCall();
        api.endpoint = this.endpointUrl;
        api.method = "configuration";

        let saveState = this._store.currencyGet();

        try {
            let data = await api.sendRequest<ICurrencyPairsResponse>();
            for (var currencyExchangeCode in data.currencyPairs) {
                let newModel = new CurrencyPairsModel();
                newModel.currencyCodeFrom = data.currencyPairs[currencyExchangeCode][0].code;
                newModel.currencyCodeTo = data.currencyPairs[currencyExchangeCode][1].code;
                newModel.currencyNameFrom = data.currencyPairs[currencyExchangeCode][0].name;
                newModel.currencyNameTo = data.currencyPairs[currencyExchangeCode][1].name;
                newModel.exchangeCode = currencyExchangeCode;
                newModel.enabledInView = saveState.length == 0 ? true : (saveState.filter(function(x) { return x === currencyExchangeCode }).length !== 0)
                
                this._currencyPairsModel.push(newModel);
            }

            this._events.notify(EEventMessages.ConfigurationLoaded, this._currencyPairsModel);
        } catch (e) {
            console.error(e);
        }
    }

    public async updateRates() {
        let pairsArray: Array<string> = this._currencyPairsModel.map(function(x) { return x.exchangeCode });
        let api = new ApiCall();
        api.endpoint = this.endpointUrl;
        api.method = "rates";
        api.addParam("currencyPairIds", pairsArray);

        try {
            var data = await api.sendRequest<IRatesResponse>();
            this._events.notify(EEventMessages.RatesRefreshed, data);
        } catch (exception) {
            this._events.notify(EEventMessages.RatesRefreshFailed);
        }
    }
}

export = AppCore