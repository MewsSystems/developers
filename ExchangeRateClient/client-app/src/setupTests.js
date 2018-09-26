// @flow

/* eslint-disable no-return-assign */
let mockStorage = {};

window.localStorage = {
  setItem: (key, val) => Object.assign(mockStorage, { [key]: val }),
  getItem: key => mockStorage[key],
  clear: () => (mockStorage = {}),
};
