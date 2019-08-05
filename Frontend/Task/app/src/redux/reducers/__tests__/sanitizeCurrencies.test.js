// @flow

import { INITIAL_STATE, ACTIONS } from '../../constants';
import sanitizeCurrencies from '../sanitizeCurrencies';

describe('sanitizeCurrencies', () => {
  it('sanitizes currencies from API', () => {
    const expectedResult = {
      ...INITIAL_STATE,
      currencyPairs: [
        {
          id: 'first-pair',
          rates: [],
          currencies: [
            { code: 'fst-code', name: 'fst-name' },
            { code: 'snd-code', name: 'smd-name' },
          ],
        },
        {
          id: 'snd-pair',
          rates: [],
          currencies: [
            { code: 'fst-code', name: 'fst-name' },
            { code: 'snd-code', name: 'smd-name' },
          ],
        },
      ],
      isLoadingConfig: false,
    };

    const result = sanitizeCurrencies(INITIAL_STATE, {
      type: ACTIONS.SANITIZE_CURRENCIES,
      payload: {
        currencyPairsApi: {
          'first-pair': [
            { code: 'fst-code', name: 'fst-name' },
            { code: 'snd-code', name: 'smd-name' },
          ],
          'snd-pair': [
            { code: 'fst-code', name: 'fst-name' },
            { code: 'snd-code', name: 'smd-name' },
          ],
        },
      },
    });

    expect(result).toEqual(expectedResult);
  });
});
