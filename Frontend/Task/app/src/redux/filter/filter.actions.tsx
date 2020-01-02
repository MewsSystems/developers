import { SEARCH_CURRENCY } from "./filter.constants";

export const searchCurrency = (value: string = "") => ({
  type: SEARCH_CURRENCY,
  payload: value
});
