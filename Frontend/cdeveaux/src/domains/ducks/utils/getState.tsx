type Action = { readonly type: string };
export const getState = (reducer, defaultState, name) => (actions: Action[]) => ({
  [name]: actions.reduce((acc, action) => reducer(acc, action), defaultState)
});
