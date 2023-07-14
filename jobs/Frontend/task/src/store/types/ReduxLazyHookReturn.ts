import { QueryData } from "src/store/types/QueryData";

export type ReduxLazyHookReturn<T> = [
  trigger: (value?: string) => void,
  {
    data: QueryData<T>;
    isLoading: boolean;
    isError: boolean;
    error: Error;
    fulfilled: boolean;
    rejected: boolean;
  },
];
