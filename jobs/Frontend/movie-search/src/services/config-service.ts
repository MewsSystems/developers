
import wretch from "wretch";
import { Config } from "../store/config-slice";
import { ConfigResponse } from "./responses/config-response";

const API_KEY = "03b8572954325680265531140190fd2a";
const BASE_API_URL = "https://api.themoviedb.org/3/";

const api = wretch(BASE_API_URL, { mode: "cors" })
  .errorType("json")
  .resolve((r) => r.json());

export const getConfigEndpoint = (): Promise<Config> => {
  return api
    .get(`configuration?api_key=${API_KEY}`)
    .then<ConfigResponse>()
    .then((data: ConfigResponse) => {
      const config: Config = {
        baseUrl: data.images.base_url,
        posterSizes: data.images.poster_sizes,
        secureBaseUrl: data.images.secure_base_url,
      };

      return config;
    });
};
