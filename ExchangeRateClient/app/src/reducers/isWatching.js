import {WATCH_TOGGLED, WATCH_STARTED} from '../const/action-names';


export default (state = false, action) => {
  switch (action.type) {
    case WATCH_STARTED:
      return true;

    case WATCH_TOGGLED:
      return !state;

    default:
      return state;
  }
};
