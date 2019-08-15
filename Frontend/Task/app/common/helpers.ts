import {CurrencyObject, ParsedCurrency, ParsedRate, RatesObject, RateType} from "../../types/app";

export const parseRates = (rates: RatesObject, currencies: ParsedCurrency[]): ParsedRate[] => {
    let result = [] as ParsedRate[];

    for (let id in rates) {
        const currency = currencies.find(entry => entry.id === id);

        if (currency) {
            result.push({
                id,
                name: `${currency.currency1.name} / ${currency.currency2.name}`,
                value: rates[id]
            });
        }
    }

    return result;
};

export const parseCurrencies = (currencies: CurrencyObject): ParsedCurrency[] => {
    let result = [] as ParsedCurrency[];

    for (let id in currencies) {
        const [currency1, currency2] = currencies[id];

        result.push({
            id,
            currency1,
            currency2
        });
    }

    return result;
};

export const findRateType = (oldValue: number, newValue: number) => {
  if (oldValue > newValue) {
      return RateType.declining;
  }

  if (newValue > oldValue) {
      return RateType.growing;
  }

  return RateType.stagnating;
};

export const compareRates = (oldRates: ParsedRate[], newRates: ParsedRate[]): ParsedRate[] => {
  for (let i = 0; i < newRates.length; i++) {
      const newRate = newRates[i];
      const oldRate = oldRates.find(entry => entry.id === newRate.id);

      if (oldRate) {
         newRates[i].type = findRateType(oldRate.value, newRate.value);
      }
  }

  return newRates;
};
