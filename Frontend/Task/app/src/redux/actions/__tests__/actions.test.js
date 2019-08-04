// @flow strict

import { ACTIONS } from '../../constants';

import * as actions from '..';

const { sanitizeCurrencies } = actions;

describe('actions', () => {
  describe('sanitizeCurrencies', () => {
    it('creates an action to sanitize currencies', () => {
      const expectedAction = {
        type: ACTIONS.SANITIZE_CURRENCIES,
        payload: {
          currencyPairsApi: {
            'test-pair': [{ code: 'test', name: 'test' }, { code: 'test', name: 'test' }],
          },
        },
      };

      const result = sanitizeCurrencies({
        currencyPairsApi: {
          'test-pair': [{ code: 'test', name: 'test' }, { code: 'test', name: 'test' }],
        },
      });

      expect(result).toEqual(expectedAction);
    });
  });
});
