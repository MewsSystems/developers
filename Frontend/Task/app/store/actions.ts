import { Dispatch } from "redux";
import { fetchApi } from "../api/api";
import { Action, CurrencyPairIds, CurrencyPairs } from "./types";
import { endpoints } from "../api/endpoints";
import { convertToParams } from "../api/api";

export const fetchExhangeRates = (params: CurrencyPairIds) => (
  dispatch: Dispatch<Action>
) => {
  const parameters = convertToParams(params);
  dispatch({
    type: "FETCH_EXCHANGE",
    async payload() {
      return fetchApi(`${endpoints.rates}${parameters}`);
    }
  });
};

export const downloadConfiguration = () => (dispatch: Dispatch<Action>) => {
  dispatch({
    type: "DOWNLOAD_CONFIG",
    async payload() {
      const config: CurrencyPairs = await fetchApi(endpoints.config);
      const allCurrencyRates: CurrencyPairIds = {
        currencyPairIds: Object.keys(config.currencyPairs)
      };
      fetchExhangeRates(allCurrencyRates);
      return config;
    }
  });
};
