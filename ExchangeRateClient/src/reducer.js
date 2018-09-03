// @flow
import { combineReducers } from 'redux-immutable';
import { Map, Record, type RecordOf, type RecordFactory } from 'immutable';
import getIn from 'lodash.get';
import { actionTypes } from './actions';

export type RequestStatus = 'init' | 'loading' | 'success' | 'failure';
export type Action = { type: string, error?: boolean, payload?: Object };
export type Currency = { code: string, name: string };

type CurrencyPairProps = {
  id: ?string,
  left: Currency,
  right: Currency,
};
export type CurrencyPair = RecordOf<CurrencyPairProps>;

type AppStateProps = {
  configRequestStatus: RequestStatus,
  currencyPairs: Map<string, CurrencyPair>,
  rates: Map<string, Array<number>>,
};
type AppState = RecordOf<AppStateProps>;

const CurrencyPairRecord: RecordFactory<CurrencyPairProps> = Record({
  id: undefined,
  left: { code: '', name: '' },
  right: { code: '', name: '' },
});
const AppStateRecord: RecordFactory<AppStateProps> = Record({
  configRequestStatus: 'init',
  currencyPairs: Map(),
  rates: Map(),
});
const appInitialState = AppStateRecord();

function app(state: AppState = appInitialState, action: Action) {
  switch (action.type) {
    case actionTypes.FETCH_CONFIG:
      return state.set('configRequestStatus', 'loading');

    case actionTypes.FETCH_CONFIG_FULFILLED: {
      const loadedCurrencyPairs = getIn(
        action,
        ['payload', 'currencyPairs'],
        {}
      );
      const currencyPairs = Map(
        Object.keys(loadedCurrencyPairs).map(id => {
          const [left, right] = loadedCurrencyPairs[id];

          return [id, CurrencyPairRecord({ id, left, right })];
        })
      );

      return state.merge({
        currencyPairs,
        configRequestStatus: 'success',
      });
    }

    case actionTypes.FETCH_RATES_FULFILLED: {
      const loadedRates = getIn(action, ['payload', 'rates'], {});
      const prevRates = state.get('rates');
      const nextRates = Object.keys(loadedRates).map(id => {
        const rate = loadedRates[id];
        const prevRate = prevRates.get(id);

        if (prevRate !== undefined) {
          return [id, [prevRate[1], rate]];
        }

        return [id, [rate, rate]];
      });

      return state.set('rates', Map(nextRates));
    }

    default:
      return state;
  }
}

type StateProps = {
  app: AppState,
};
export type State = RecordOf<StateProps>;

export const StateRecord: RecordFactory<StateProps> = Record({
  app: appInitialState,
});

export default combineReducers({ app }, StateRecord);
