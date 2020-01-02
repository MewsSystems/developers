import { createSelector } from "reselect";
import { loadState } from "../../utils";

const selectConfig = state => state.configuration.currencies;
const selectSearchTerm = state => state.filter.searchTerm;

export const filterSearch = createSelector(
  selectConfig,
  selectSearchTerm,
  (config, searchTerm) => {
    return Object.keys(config).filter(id => id.includes(loadState("select")));
  }
);

export const getFilteredCurrencies = createSelector(
  selectConfig,
  filterSearch,
  (config, id) => {
    return config[id];
  }
);
