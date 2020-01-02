import { createSelector } from "reselect";
import { loadState } from "../../utils";

const selectConfig = state => state.configuration.currencies;
const selectSearchTerm = state => state.filter.searchTerm;

const getIds = createSelector(selectConfig, currencyPairs =>
  Object.keys(currencyPairs)
);

export const filterSearch = createSelector(
  getIds,
  selectSearchTerm,
  (ids, searchTerm) => {
    return ids.filter(id =>
      id.includes(loadState("select") ? loadState("select") : searchTerm)
    );
  }
);

export const getFilteredCurrencies = createSelector(
  selectConfig,
  filterSearch,
  (config, ids) => {
    const result = {};
    ids.map(
      id =>
        (result[id] = [
          { name: config[id][0].name, code: config[id][0].code },
          { name: config[id][1].name, code: config[id][1].code }
        ])
    );
    return result;
  }
);
