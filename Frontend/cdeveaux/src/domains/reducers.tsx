import { ThunkDispatch } from 'redux-thunk';
import { MiddlewareAPI, combineReducers } from 'redux';

import assets, { Action as AssetsAction } from './ducks/assets';
import config, { Action as ConfigAction } from './ducks/config';

const rootReducer = combineReducers({
  assets,
  config,
});

export type GeneralAction =
  | ConfigAction
  | AssetsAction;


export type RootState = ReturnType<typeof rootReducer>;
export type Dispatch = ThunkDispatch<RootState, void, GeneralAction>;
export type GetState = () => RootState;
export type MiddlewareStore = MiddlewareAPI<Dispatch, RootState>;
export default rootReducer;
