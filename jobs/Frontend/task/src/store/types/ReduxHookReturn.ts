export type ReduxHookReturn<T> = {
  data: T;
  isFetching: boolean;
  isError: boolean;
  error: Error;
  fulfilled: boolean;
  rejected: boolean;
};
