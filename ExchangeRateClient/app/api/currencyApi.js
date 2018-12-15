import { get } from "./index.js";

export function getConfigFromApi() {
  return get("/configuration");
}

export function getRatesFromApi(currencyPairs) {
  const config = {
    params: {
      currencyPairIds: currencyPairs
    }
  };

  return get("/rates", config);
}
