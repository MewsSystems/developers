import { SEARCH_CURRENCY } from "./filter.constants";

export interface ISearchCurrency {
  type: typeof SEARCH_CURRENCY;
  payload: string;
}

export interface FilterState {
  searchTerm: string;
}

export type FilterAction = ISearchCurrency;
