import { QueryData } from "src/store/types/QueryData";

export type ReduxHookReturn<T> = {
  data: QueryData<T>;
  isFetching: boolean;
  isError: boolean;
  error: Error;
  fulfilled: boolean;
  rejected: boolean;
};
