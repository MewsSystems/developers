import { get } from "./index.js";

export function getConfiguration() {
  return get("/configuration");
}
