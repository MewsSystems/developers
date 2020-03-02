// @ts-nocheck
export const getState = (reducer, defaultState, name) => (actions) => ({
  [name]: actions.reduce((acc, action) => reducer(acc, action), defaultState)
});
