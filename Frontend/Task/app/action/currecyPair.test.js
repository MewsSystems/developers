import {
  getCurrencyPairShortcut,
  getCurrencyPairWithShortcut,
  getPairCollectionWithShortcut,
  iteratePairCollection,
} from './currencyPair';
import stateWithExchangeRateFixture from '../fixture/stateWithExchangeRateFixture';
import stateWithExchangeRateWithShortcutsFixture from '../fixture/stateWithExchangeRateWithShortcutsFixture';

describe('Currency pair getter test', () => {
  it('Get currency pair shortcut', () => {
    const pairs = getCurrencyPairShortcut(stateWithExchangeRateFixture.id1.pair);
    expect(pairs).toEqual('EUR/USD');
  });

  it('Get currency pair with shortcut', () => {
    const pairs = getCurrencyPairWithShortcut(stateWithExchangeRateFixture.id2);
    expect(pairs).toEqual(stateWithExchangeRateWithShortcutsFixture.id2);
  });

  it('Get currency pairs with shortcuts', () => {
    const pairs = getPairCollectionWithShortcut(stateWithExchangeRateFixture);
    expect(pairs).toEqual(stateWithExchangeRateWithShortcutsFixture);
  });

  it('Iterate pairs', () => {
    const callback = jest.fn((key, value) => ({ key, value }));
    iteratePairCollection(stateWithExchangeRateFixture, callback);
    expect(callback.mock.calls.length).toBe(2);
    expect(callback.mock.calls[0][0]).toBe('id1');
    expect(callback.mock.calls[1][0]).toBe('id2');
    expect(callback.mock.results[0].value).toEqual({
      key: 'id1',
      value: {
        pair: [{ code: 'EUR', name: 'Euro' }, { code: 'USD', name: 'US Dollar' }],
        rate: 1.102,
        trend: 'growing',
      },
    });
  });
});
