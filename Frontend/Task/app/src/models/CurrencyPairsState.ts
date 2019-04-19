import CurrencyPairsModel from './CurrencyPairsModel'

class CurrencyPairsState {
    public currencies: Array<CurrencyPairsModel> = [];
    public visibility: boolean = false;
}

export = CurrencyPairsState;