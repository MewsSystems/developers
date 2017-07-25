const savedStorage = sessionStorage ? JSON.parse(sessionStorage.getItem('appState')) : null

const initialState = savedStorage || {
  exchangeRatesReducer: {
    filterValue: [],
    isFilterEnabled: false,
  },
}

export default initialState
