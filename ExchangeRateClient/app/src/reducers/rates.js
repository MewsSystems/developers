import {RATES_LOADED} from '../const/action-names';


const defaultState = {
  prev: {},
  cur: {}
};
export default (state = defaultState, action) => (
  action.type === RATES_LOADED ? {
      prev: state.cur,
      cur: action.payload
    } : state
);
