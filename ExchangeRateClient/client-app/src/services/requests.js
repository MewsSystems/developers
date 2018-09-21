// @flow strict
import axios from "axios";
import axiosRetry from "axios-retry";
import sanitizeConfiguration from "../utils/sanitizeConfiguration";
import config from "../config";

const client = axios.create({ baseURL: config.endpoint, timeout: config.requestsTimeout });
axiosRetry(client, { retries: 0 });

export async function fetchConfiguration() {
  return client.get("/configuration").then(resp => sanitizeConfiguration(resp.data));
}

export async function fetchRates(currencyPairIds: Array<string>) {
  return client
    .get("/rates", {
      params: {
        currencyPairIds,
      },
      "axios-retry": {
        retries: 2,
      },
    })
    .then(({ data }) => data.rates);
}
