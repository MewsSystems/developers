// @flow
import { combineReducers } from 'redux-immutable';
import { Map, Record, type RecordOf, type RecordFactory } from 'immutable';
import getIn from 'lodash.get';
import { actionTypes } from './actions';

type RequestStatus = 'init' | 'loading' | 'success' | 'failure';
type Action = { type: string, error?: boolean, payload?: Object };
type Currency = { code: string, name: string };
type CurrencyPair = [Currency, Currency];
type AppStateProps = {
  configRequestStatus: RequestStatus,
  currencyPairs: Map<string, CurrencyPair>,
  rates: Map<string, number>,
};
type AppState = RecordOf<AppStateProps>;

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
      const currencyPairs = getIn(action, ['payload', 'currencyPairs'], {});

      return state.merge({
        configRequestStatus: 'success',
        currencyPairs: Map(currencyPairs),
      });
    }

    case actionTypes.FETCH_RATES_FULFILLED: {
      const rates = getIn(action, ['payload', 'rates'], {});

      return state.mergeIn(['rates'], Map(rates));
    }

    default:
      return state;
  }
}

type StateProps = {
  app: AppState,
};

export const StateRecord: RecordFactory<StateProps> = Record({
  app: appInitialState,
});

export type State = RecordOf<StateProps>;

export default combineReducers({ app }, StateRecord);
