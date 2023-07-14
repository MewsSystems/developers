import { MovieQueryParams } from "src/store/slices/moviesSlice";
import { QueryData } from "src/store/types/QueryData";
import { ReduxHookPromiseData } from "src/store/types/ReduxHookPromiseData";

export type ReduxLazyHookReturn<T> = [
  fetch: (params: MovieQueryParams) => Promise<ReduxHookPromiseData<T>>,
  {
    data: QueryData<T>;
    isLoading: boolean;
    isError: boolean;
    error: Error;
    fulfilled: boolean;
    rejected: boolean;
  },
];
