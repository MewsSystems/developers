import constants from '../constants';

const initialState = {
  currencyPairs: undefined,
};

const reducer = (state = initialState, action) => {
  switch (action.type) {
    case constants.FETCH_CONFIG_SUCCESS: {
      const currencyPairs = action.payload.currencyPairs;
      
      const pairsIds = Object.entries(currencyPairs);
      const options = pairsIds.map(pair => {
        return {
          value: pair[0],
          label: pair[1]
        }
      });
      console.log(pairsIds);
      
      return {
        ...state,
        currencyPairs
      }
    }
    default: {
      return state;
    }
  }
}

export default reducer;
