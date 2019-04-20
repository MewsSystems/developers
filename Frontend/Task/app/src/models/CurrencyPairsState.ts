import CurrencyPairsModel from './CurrencyPairsModel'

class CurrencyPairsState {
    public currencies: Array<CurrencyPairsModel> = [];
    public visibility: boolean = false;
    public display() {
        return this.visibility ? "block" : "none";
    }
    public downloadError: boolean = false;
}

export = CurrencyPairsState;