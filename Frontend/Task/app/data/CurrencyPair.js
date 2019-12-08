export class CurrencyPair {
    constructor() {
        this.id = null;
        this.frcode = null;
        this.frname = null;
        this.tocode = null;
        this.toname = null;
        this.trend = "?";
        this.rate = null;
        this.selected = false;
    }

    clone() {
        const currencyPair = new CurrencyPair();
        currencyPair.id = this.id;
        currencyPair.frcode = this.frcode;
        currencyPair.frname = this.frname;
        currencyPair.tocode = this.tocode;
        currencyPair.toname = this.toname;
        currencyPair.trend = this.trend;
        currencyPair.rate = this.rate;
        currencyPair.selected = this.selected;
        return currencyPair;
    }

    static fromCursor(cursor) {
        const currencyPair = new CurrencyPair();
        currencyPair.id = cursor.value.id;
        currencyPair.frname = cursor.value.frname;
        currencyPair.frcode = cursor.value.frcode;
        currencyPair.toname = cursor.value.toname;
        currencyPair.tocode = cursor.value.tocode;
        currencyPair.selected = cursor.value.selected;
        return currencyPair;
    }

    static fromRest(id, data) {
        const currencyPair = new CurrencyPair();
        currencyPair.id = id;
        currencyPair.frname = data[0].name;
        currencyPair.frcode = data[0].code;
        currencyPair.toname = data[1].name;
        currencyPair.tocode = data[1].code;
        return currencyPair;
    }
}
