import { PaginationQueryData } from "src/store/types/PaginationQueryData";

export interface ReduxHookPromiseData<T> {
  data: PaginationQueryData<T>;
  endpointName: string;
  fulfilledTimeStamp: number;
  isError: boolean;
  isLoading: boolean;
  isSuccess: boolean;
  isUninitialized: boolean;
  originalArgs: {
    name: string;
  };
  requestId: string;
  startedTimeStamp: number;
  status: string;
}
