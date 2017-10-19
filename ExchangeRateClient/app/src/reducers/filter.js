import {FILTER_CHANGED} from '../const/action-names';


export default (state = [], action) => {
  if (action.type !== FILTER_CHANGED) {
    return state;
  }

  const id = action.payload;
  const index = state.indexOf(id);

  return ~index ? state.filter(item => item !== id) : [...state, id];
};
