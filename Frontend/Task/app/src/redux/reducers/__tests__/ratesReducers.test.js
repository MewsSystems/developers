// @flow

import { INITIAL_STATE, ACTIONS } from '../../constants';
import { addRates } from '../ratesReducers';

describe('addRates', () => {
  it('adds new rates to currency pairs', () => {
    const expectedResult = {
      ...INITIAL_STATE,
      currencyPairs: [
        {
          id: 'first-pair',
          rates: [1.5],
          currencies: [
            { code: 'fst-code', name: 'fst-name' },
            { code: 'snd-code', name: 'smd-name' },
          ],
        },
        {
          id: 'snd-pair',
          rates: [1.5, 2.5],
          currencies: [
            { code: 'fst-code', name: 'fst-name' },
            { code: 'snd-code', name: 'smd-name' },
          ],
        },
      ],
      isLoadingRates: false,
    };

    const result = addRates(
      {
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
            rates: [1.5],
            currencies: [
              { code: 'fst-code', name: 'fst-name' },
              { code: 'snd-code', name: 'smd-name' },
            ],
          },
        ],
      },
      {
        type: ACTIONS.ADD_RATES,
        payload: {
          ratesApi: {
            'first-pair': 1.5,
            'snd-pair': 2.5,
          },
        },
      },
    );

    expect(result).toEqual(expectedResult);
  });
});
