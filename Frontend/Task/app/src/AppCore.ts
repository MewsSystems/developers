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

class AppCore extends Singleton {
    private _endpointUrl: string;
    private _events: EventAggregator = <EventAggregator>EventAggregator.instance;

    private _currencyPairsModel: Array<CurrencyPairsModel> = [];

    constructor() {
        super();
        this._endpointUrl = endpoint.replace("/rates", "");
        
        this._events.subscribe(this.eaListener.bind(this));

        //@todo - remove
        (<any>window)["ea"] = this._events;
    }

    public get endpointUrl(): string {
        return this._endpointUrl;
    }

    public eaListener(message: EEventMessages, value: any) {
        if (message == EEventMessages.CurrencyDisplayChanged) {
            let data = value as ICurrencyDisplayChanged;
            if (data.newState) {
                this.storeCurrencyAdd(data.exchangeCode);
            } else {
                this.storeCurrencyRemove(data.exchangeCode);
            }
        }
    }

    public async render() {
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
    }

    //@todo -> přesunout do samostatné třídy
    public getSaveState(): Array<string> {
        let response: Array<string> = [];
        try {
            let saveStateCurrencyPairsSelection = window.localStorage.getItem("currencyPairsSelection");
            if (saveStateCurrencyPairsSelection !== null) {
                response = JSON.parse(saveStateCurrencyPairsSelection);
            }
        } catch (e) {
            console.error("store-expection");
        }

        return response;
    }
    public storeCurrencyAdd(key: string) {
        let data = this.getSaveState();
        data = data.filter(function(x) { return x !== key });
        data.push(key);
        window.localStorage.setItem("currencyPairsSelection", JSON.stringify(data));
    }
    public storeCurrencyRemove(key: string) {
        let data = this.getSaveState();
        data = data.filter(function(x) { return x !== key });
        window.localStorage.setItem("currencyPairsSelection", JSON.stringify(data));
    }
    // --- store ---

    private async getConfiguration() {
        let api = new ApiCall();
        api.endpoint = this.endpointUrl;
        api.method = "configuration";

        let saveState = this.getSaveState();

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
            console.error(exception);
        }
    }
}

export = AppCore