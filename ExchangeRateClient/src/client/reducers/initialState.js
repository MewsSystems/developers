const savedStorage = localStorage ? JSON.parse(localStorage.getItem('appState')) : null

const initialState = savedStorage || {
  exchangeRatesReducer: {
    filterValue: [],
    isFilterEnabled: false,
  },
}

export default initialState
