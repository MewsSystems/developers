import axios from "axios";
import { SERVICE_URL } from "../config";

let api = null;

function getInitializedApi() {
  if (api) return api; // return initialized api if already initialized.
  api = axios.create({
    baseURL: getBaseUrl(),
    responseType: "json"
  });
  return api;
}

function getBaseUrl() {
  return SERVICE_URL;
}

export function get(url, config) {
  return getInitializedApi().get(url, config);
}

export function post(url, data, config) {
  return getInitializedApi().post(url, data, config);
}
