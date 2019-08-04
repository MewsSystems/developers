// @flow strict

import { ACTIONS } from '../../constants';

import * as actions from '..';

const { sanitizeCurrencies, fetchCurrenciesConfigPending, fetchCurrenciesConfigError } = actions;

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

  describe('fetchCurrenciesConfigPending', () => {
    it('creates an action to fetch currencies config success', () => {
      const expectedAction = {
        type: ACTIONS.FETCH_CURRENCIES_CONFIG_PENDING,
        payload: {},
      };

      expect(fetchCurrenciesConfigPending()).toEqual(expectedAction);
    });
  });

  describe('fetchCurrenciesConfigError', () => {
    it('creates an action to fetch currencies config errorr', () => {
      const expectedAction = {
        type: ACTIONS.FETCH_CURRENCIES_CONFIG_ERROR,
        payload: {
          error: new Error('test error'),
        },
      };

      const result = fetchCurrenciesConfigError({
        error: new Error('test error'),
      });

      expect(result).toEqual(expectedAction);
    });
  });
});
