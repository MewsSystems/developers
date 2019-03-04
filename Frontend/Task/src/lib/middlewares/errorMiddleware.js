import isPromise from 'is-promise';

export default function errorMiddleware() {
  return next => action => {
    // If not a promise, continue on
    if (!isPromise(action.payload)) {
      return next(action);
    }

    // Dispatch initial pending promise, but catch any errors
    return next(action).catch(error => {
      console.warn(error);

      return error;
    });
  };
}
