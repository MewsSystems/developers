import { Routes } from '../constants';
import { buildCurrencyPairsIdsQuery } from '../../utils';

export default {
  updateRates: (currencyPairsIds) => {
    const query = buildCurrencyPairsIdsQuery(currencyPairsIds);
    const url = `${Routes.Rates}?${query}`

    return fetch(url)
      .then((res) => {
        if (res.ok) {
          return res.json();
        } else {
          throw 'Cannot fetch rates';
        }

      });
  }
}