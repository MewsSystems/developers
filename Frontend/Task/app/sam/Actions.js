import $ from "jquery";

import {
    ToggleCurrencyPair,
    Init,
    ConfigureCurrencyPairs,
    LoadCurrencyPairs,
    UpdateCurrencyPairValues,
    DecrementCounter,
    FailCurrencyPairUpdate,
    FailConfigurationLoad
} from "./Proposals";

import { model } from "./Model";
import { configurationRepo } from "../data/Configuration";
import { CurrencyPair } from "../data/CurrencyPair";

export const toggleCurrencyPair = currencyPairId => {
    model.accept(new ToggleCurrencyPair(currencyPairId));
};

export const init = (endpoint, interval) => {
    model.accept(new Init(endpoint, interval));
};

export const loadConfiguration = () => {
    configurationRepo
        .getConfiguration()
        .then(currencyPairsById => {
            model.accept(new LoadCurrencyPairs(currencyPairsById));
        })
        .catch(error => {
            console.error(error);
            model.accept(new FailConfigurationLoad());
        });
};

export const fetchCurrencyPairs = () => {
    $.get("http://localhost:3000/configuration")
        .done(data => {
            const result = {};
            for (let [id, data] of Object.entries(data.currencyPairs)) {
                result[id] = CurrencyPair.fromRest(id, data);
            }
            model.accept(new ConfigureCurrencyPairs(result));
        })
        .fail(error => {
            console.log(error);
        });
};

export const updateCurrencyPairValues = (endpoint, currencyPairIds) => {
    setTimeout(() => {
        $.get(endpoint, { currencyPairIds })
            .done(data => {
                model.accept(new UpdateCurrencyPairValues(data.rates));
            })
            .fail(() => {
                model.accept(new FailCurrencyPairUpdate());
            });
    }, 500);
};

export const decrementCounter = () => {
    setTimeout(() => {
        model.accept(new DecrementCounter());
    }, 1000);
};
