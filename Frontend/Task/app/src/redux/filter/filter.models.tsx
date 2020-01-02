import { SEARCH_CURRENCY } from "./filter.constants";

export interface ISearchCurrency {
  type: typeof SEARCH_CURRENCY;
  payload: string;
}
export type FilterState = {
  searchTerm: string;
};

export type FilterProps = {
  handleChange: (e: React.ChangeEvent<HTMLSelectElement>) => void;
  value: string;
};
export type FilterAction = ISearchCurrency;
