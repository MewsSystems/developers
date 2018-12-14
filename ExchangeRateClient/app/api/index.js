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

export function get(url) {
  return getInitializedApi().get(url);
}

export function post(url, data, config) {
  return getInitializedApi().post(url, data, config);
}
