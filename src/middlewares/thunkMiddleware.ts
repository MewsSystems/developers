export default ({ dispatch, getState }: any) =>
  (next: any) =>
  (action: any) => {
    return typeof action === 'function' ? action(dispatch, getState) : next(action);
  };
