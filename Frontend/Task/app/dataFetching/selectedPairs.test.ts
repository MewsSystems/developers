/* eslint-disable @typescript-eslint/ban-ts-ignore */
import { loadSelectedPairs, storeSelectedPairs } from './selectedPairs';

describe('storeSelectedPairs', () => {
  it('saves to the localStorage', () => {
    const setItem = jest.fn();
    // @ts-ignore
    global.localStorage = {
      setItem,
    };

    storeSelectedPairs(['aaa', 'bbb']);

    expect(setItem.mock.calls).toEqual([['selectedPairs', '["aaa","bbb"]']]);
  });

  it('returns the argument', () => {
    // @ts-ignore
    global.localStorage = {
      setItem: jest.fn(),
    };

    const argument = ['aaa', 'bbb'];
    const returnedValue = storeSelectedPairs(argument);
    expect(returnedValue).toEqual(argument);
  });
});

describe('loadSelectedPairs', () => {
  it('loads empty array on empty storage', () => {
    const getItem = jest.fn(() => {
      return null;
    });
    // @ts-ignore
    global.localStorage = {
      getItem,
    };

    const loadedPairs = loadSelectedPairs();

    expect(loadedPairs).toEqual([]);
    expect(getItem.mock.calls).toEqual([['selectedPairs']]);
  });

  it('loads the IDs from the storage', () => {
    const getItem = jest.fn(() => {
      return '["aaa","bbb"]';
    });
    // @ts-ignore
    global.localStorage = {
      getItem,
    };

    const loadedPairs = loadSelectedPairs();

    expect(loadedPairs).toEqual(['aaa', 'bbb']);
    expect(getItem.mock.calls).toEqual([['selectedPairs']]);
  });

  it("doesn't crash on invalid JSON", () => {
    const getItem = jest.fn(() => {
      return '¯\\_(ツ)_/¯';
    });
    // @ts-ignore
    global.localStorage = {
      getItem,
    };

    const loadedPairs = loadSelectedPairs();

    expect(loadedPairs).toEqual([]);
    expect(getItem.mock.calls).toEqual([['selectedPairs']]);
  });
});
