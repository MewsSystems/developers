import { GetMoviesParams } from "src/store/slices/interfaces/GetMoviesParams";
import { ReduxHookPromiseData } from "src/store/types/ReduxHookPromiseData";

export type ReduxLazyHookReturn<T> = [
  fetch: (params: GetMoviesParams) => Promise<ReduxHookPromiseData<T>>,
  {
    data: T;
    isLoading: boolean;
    isError: boolean;
    error: Error;
    fulfilled: boolean;
    rejected: boolean;
  },
];
