import test from 'ava';
import { RATES } from '../constants/actionTypes';

import reducer from './rates';

test('Reducer should return initial state', t => {
  t.deepEqual(reducer(undefined, {}), {
    isLoading: false,
    isError: false,
    selected: [],
    current: {},
    previous: {},
  });
});

test('Reducer should handle Request action', t => {
  t.deepEqual(reducer({}, {
    type: `REQUEST_${RATES}`,
  }), {
    isLoading: true,
    isError: false,
  });
});

test('Reducer should handle Success action', t => {
  t.deepEqual(reducer([], {
    type: `SUCCESS_${RATES}`,
    payload: {
      rates: {
        1: 1,
        2: 2,
      },
    },
  }), {
    isLoading: false,
    isError: false,
    current: {
      1: 1,
      2: 2,
    },
    previous: undefined,
  });
});

test('Reducer should handle Error action', t => {
  t.deepEqual(reducer([], {
    type: `ERROR_${RATES}`,
  }), {
    isError: true,
    isLoading: false,
  });
});
