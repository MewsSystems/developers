import { present } from "./Presenter";
import { configurationRepo } from "../data/Configuration";

export const CONTROL_STATES = {
    TIME_TO_UPDATE: 0,
    DECREMENT: 1,
    NO_ACTION: 2,
    FIRST_FETCH: 3,
    GET_CONFIGURATION: 4,
    GET_CONFIGURATION_FROM_DB: 5
};

export class Model {
    constructor() {
        this.endpoint = null;
        this.interval = null;
        this.currencyPairsById = {};
        this.counter = 0;
        this.lastFailed = false;
        this.control = CONTROL_STATES.NO_ACTION;
        this._persist = false;
    }

    accept(proposal) {
        this._persist = false;
        console.debug(proposal);
        this[proposal.name](proposal);
        present(this);

        if (this._persist) {
            this._persistPairs();
        }
    }

    init(proposal) {
        this.endpoint = proposal.endpoint;
        this.interval = proposal.interval;
        this.control = CONTROL_STATES.GET_CONFIGURATION_FROM_DB;
    }

    toggleCurrencyPair(proposal) {
        this.control = CONTROL_STATES.NO_ACTION;
        const pair = this.currencyPairsById[proposal.currencyPairId];
        pair.selected = !pair.selected
        this._persist = true;
    }

    failConfigurationLoad() {
        this.control = CONTROL_STATES.GET_CONFIGURATION;
    }

    loadCurrencyPairs(proposal) {
        this.configureCurrencyPairs(proposal);
        this._persist = false;
    }

    configureCurrencyPairs(proposal) {
        this.currencyPairsById = proposal.currencyPairsById;
        this.control = Object.values(this.currencyPairsById).length
            ? CONTROL_STATES.FIRST_FETCH
            : CONTROL_STATES.GET_CONFIGURATION;
        this._persist = true;
    }

    updateCurrencyPairValues(proposal) {
        this.lastFailed = false;
        const rates = proposal.rates;
        this.control = CONTROL_STATES.DECREMENT;
        this._resetCounter();
        for (let [id, rate] of Object.entries(rates)) {
            const pair = this.currencyPairsById[id];
            const prevRate = pair.rate;
            if (prevRate) {
                const prevRounded = Math.floor(prevRate * 1000);
                const newRounded = Math.floor(rate * 1000);
                if (prevRounded < newRounded) {
                    pair.trend = "^";
                } else if (prevRounded > newRounded) {
                    pair.trend = "v";
                } else {
                    pair.trend = "=";
                }
            }
            pair.rate = rate;
        }
    }

    failCurrencyPairUpdate() {
        this.lastFailed = true;
        this._resetCounter();
        if (this.control !== CONTROL_STATES.FIRST_FETCH) {
            this.control = CONTROL_STATES.DECREMENT;
        }
    }

    decrementCounter() {
        this.counter -= 1;
        this.control =
            this.counter === 0 ? CONTROL_STATES.TIME_TO_UPDATE : CONTROL_STATES.DECREMENT;
    }

    _resetCounter() {
        this.counter = Math.floor(this.interval / 1000);
    }

    _persistPairs() {
        configurationRepo.storeConfiguration(this.currencyPairsById);
    }
}

export const model = new Model();
