import {createSelector} from 'reselect'


const selectConfig = (state) => state.configuration.currencies
const selectSearchTerm = (state) => state.configuration.searchTerm


const getArrayFromObject = createSelector(
  selectConfig,
  (config) => {
    return Object.entries(config).map(e =>({id: e[0], name: e[1].name, code: e[1].code}))
  }
)

export const filterSearch = createSelector(
  getArrayFromObject,
  selectSearchTerm,
  (arrayFromObject, searchTerm) => {
      return arrayFromObject.filter(val => val.name.toLowerCase().indexOf(searchTerm) != -1)

  }
)

export const getFilteredCurrencies = createSelector(
  filterSearch,
  (pairs) => {
    return pairs.map(val => {
      return {
        id: val.id,
        name: val.name,
        code: val.code
      }
    })
  }
)
