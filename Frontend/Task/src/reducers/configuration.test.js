import test from 'ava';
import { CONFIGURATION } from '../constants/actionTypes';

import reducer from './configuration';

test('Reducer should return initial state', t => {
  t.deepEqual(reducer(undefined, {}), {
    isError: false,
    isLoading: false,
  });
});

test('Reducer should handle Request action', t => {
  t.deepEqual(reducer([], {
    type: `REQUEST_${CONFIGURATION}`,
  }), {
    isError: false,
    isLoading: true,
  });
});

test('Reducer should handle Success action', t => {
  t.deepEqual(reducer([], {
    type: `SUCCESS_${CONFIGURATION}`,
    payload: {
      currencyPairs: {
        1: [],
        2: [],
      },
    },
  }), {
    currencyPairs: {
      1: [],
      2: [],
    },
    isLoading: false,
    isError: false,
  });
});

test('Reducer should handle Error action', t => {
  t.deepEqual(reducer([], {
    type: `ERROR_${CONFIGURATION}`,
  }), {
    isError: true,
    isLoading: false,
  });
});
