import * as actionTypes from './actionTypes';

export const fetchConfigInit = () => {
  return {
    type: actionTypes.FETCH_CONFIG_INIT
  };
};

export const checkConfig = () => {
  return {
    type: actionTypes.CHECK_CONFIG
  };
};

export const fetchConfig = config => {
  return {
    type: actionTypes.FETCH_CONFIG,
    payload: config
  };
};

export const fetchConfigSuccess = config => {
  return {
    type: actionTypes.FETCH_CONFIG_SUCCESS,
    payload: config
  };
};

export const fetchConfigFail = error => {
  return {
    type: actionTypes.FETCH_CONFIG_FAIL,
    payload: error
  };
};
