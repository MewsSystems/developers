import {FILTER_CHANGED, FILTER_CLEARED} from '../const/action-names';


export default (state = [], action) => {
  switch (action.type) {
    case FILTER_CLEARED:
      return [];

    case FILTER_CHANGED:
      const id = action.payload;
      const index = state.indexOf(id);
      return ~index ? state.filter(item => item !== id) : [...state, id];

    default:
      return state;
  }
};
