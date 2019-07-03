import * as actionTypes from '../actions/actionTypes';
import { fetchConfigSuccessInterface } from '../actions/types';
import { ConfigInterface } from '../../types';

const initialState = {};

const reducer = (
  state: ConfigInterface = initialState,
  action: fetchConfigSuccessInterface
) => {
  switch (action.type) {
    case actionTypes.FETCH_CONFIG_SUCCESS:
      return action.payload;
    default:
      return state;
  }
};

export default reducer;
