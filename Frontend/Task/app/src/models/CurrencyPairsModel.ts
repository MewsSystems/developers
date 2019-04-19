import EExchangeRateTrend from "../abstracts/EExchangeRateTrend"

class CurrencyPairsModel {
    public exchangeCode: string = "";
    public currencyCodeFrom: string = "";
    public currencyCodeTo: string = "";
    public currencyNameFrom: string = "";
    public currencyNameTo: string = "";

    public enabledInView: boolean = true;

    public trend(): EExchangeRateTrend {
        let trend = this._lastRate - this._rate;
        let response = EExchangeRateTrend.EQUAL
        if (trend > 0) {
            response = EExchangeRateTrend.DOWN;
        } else if (trend < 0) {
            response = EExchangeRateTrend.UP;
        }

        return response;
    }

    private _rate: number = 0;
    public set rate(value: number) {
        this._lastRate = this._rate;
        this._rate = value;
    }
    public get rate(): number {
        return this._rate;
    }

    private _lastRate: number = 0;

    public get currencyTitle(): string {
        return this.currencyNameFrom + "/" + this.currencyNameTo;
    }
}

export = CurrencyPairsModel;