class Proposal {
    constructor(name) {
        this.name = name;
    }
}

export class ToggleCurrencyPair extends Proposal {
    constructor(currencyPairId) {
        super("toggleCurrencyPair");
        this.currencyPairId = currencyPairId;
    }
}

export class Init extends Proposal {
    constructor(endpoint, interval) {
        super("init");
        this.endpoint = endpoint;
        this.interval = interval;
    }
}

export class ConfigureCurrencyPairs extends Proposal {
    constructor(currencyPairsById) {
        super("configureCurrencyPairs");
        this.currencyPairsById = currencyPairsById;
    }
}

export class UpdateCurrencyPairValues extends Proposal {
    constructor(rates) {
        super("updateCurrencyPairValues");
        this.rates = rates;
    }
}

export class DecrementCounter extends Proposal {
    constructor() {
        super("decrementCounter");
    }
}

export class FailCurrencyPairUpdate extends Proposal {
    constructor() {
        super("failCurrencyPairUpdate");
    }
}

export class FailConfigurationLoad extends Proposal {
    constructor() {
        super("failConfigurationLoad");
    }
}

export class LoadCurrencyPairs extends Proposal {
    constructor(currencyPairsById) {
        super("loadCurrencyPairs");
        this.currencyPairsById = currencyPairsById;
    }
}