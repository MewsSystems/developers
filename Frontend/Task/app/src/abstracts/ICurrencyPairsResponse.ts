interface ICurrencyPairsResponse {
    currencyPairs: { [key: string] : [ICurrecy, ICurrecy] }
}

interface ICurrecy {
    code: string,
    name: string
}

export = ICurrencyPairsResponse