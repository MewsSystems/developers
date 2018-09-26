// @flow
import logger from "./services/logger";
import type { StateTypes } from "./store/types";

export const loadState = () => {
  try {
    const serializedState = localStorage.getItem("state");
    if (!serializedState) {
      return undefined;
    }
    return JSON.parse(serializedState);
  } catch (err) {
    return undefined;
  }
};

export const saveState = (state: StateTypes) => {
  try {
    const serializedState = JSON.stringify(state);
    localStorage.setItem("state", serializedState);
  } catch (err) {
    logger(err);
  }
};
