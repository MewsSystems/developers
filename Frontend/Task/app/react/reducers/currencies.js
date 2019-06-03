import constants from '../constants';

const initialState = {
  currencyPairs: undefined,
  options: [],
  selectedPair: undefined,
  loadingConfig: false,
  rates: [],
  intervalId: null,
  newIntervalId: null,
};

const reducer = (state = initialState, action) => {
  switch (action.type) {
    case constants.FETCH_CONFIG_START: {
      return {
        ...state,
        loadingConfig: true
      }
    }
    case constants.FETCH_CONFIG_SUCCESS: {
      const currencyPairs = action.payload.currencyPairs;
      const pairsIds = Object.keys(currencyPairs);
      const options = [];
      
      for (let i = 0; i < pairsIds.length; i++) {
        options.push({
          value: pairsIds[i],
          label: currencyPairs[pairsIds[i]].map(x => x.name + ' (' + x.code + ')').join(' / ')
        });
      }
      
      window.localStorage.setItem('config', JSON.stringify({currencyPairs, options}));
      
      return {
        ...state,
        currencyPairs,
        options,
        loadingConfig: false
      }
    }
    
    case constants.FETCH_RATES_SUCCESS: {
      const { rates } = action.payload;
      
      if (rates) {
        const oldRates = state.rates;
        const newRates = Object.entries(rates).map(x => ({
          id: x[0],
          rate: x[1],
          label: state.currencyPairs[x[0]].map(x => x.code).join(' / ')
        }));
        
        if (oldRates.length > 0) {
          for (let i = 0; i < newRates.length; i++) {
            if (oldRates.find(oldRate => oldRate.id === newRates[i].id)?.rate < newRates[i].rate) {
              newRates[i].dynamic = 'growing'
            } else if (oldRates.find(oldRate => oldRate.id === newRates[i].id)?.rate > newRates[i].rate) {
              newRates[i].dynamic = 'declining'
            } else {
              newRates[i].dynamic = 'stagnating'
            }
          }
        }
  
        return {
          ...state,
          rates: newRates
        };
      }
      
      return state;
    }
    
    case constants.RESTORE_CONFIG: {
      const { config, selectedPair } = action.payload;
      return {
        ...state,
        ...config,
        selectedPair
      }
    }
    
    case constants.SET_SELECT_VALUE: {
      return {
        ...state,
        selectedPair: action.payload
      }
    }
    
    case constants.SET_INTERVAL_ID: {
      console.log('newIntervalId', action.payload);
      return {
        ...state,
        intervalId: action.payload,
        // newIntervalId: action.payload,
      }
    }
    
    case constants.CLEAR_DATA: {
      return {
        ...state,
        rates: []
      }
    }
    
    default: {
      return state;
    }
  }
}

export default reducer;
