import {CONFIG_LOADED} from '../const/action-names';

export default (state = {}, action) => (
  action.type === CONFIG_LOADED
    ? action.payload
    : state
);
