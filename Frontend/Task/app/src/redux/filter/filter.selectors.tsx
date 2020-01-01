import {createSelector} from 'reselect'
import { loadState } from '../../utils'

const selectConfig = (state) => state.configuration.currencies
const selectSearchTerm = (state) => state.filter.searchTerm

const getArrayFromObject = createSelector(
  selectConfig,
  (config) => {
    return Object.entries(config).map(el =>({id: el[0], name: el[1].name, code: el[1].code}))
  }
)

export const filterSearch = createSelector(
  getArrayFromObject,
  selectSearchTerm,
  (arrayFromObject) => {
      return arrayFromObject.filter(val =>  val.name.includes(loadState("select")))

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
