import { QueryData } from "src/store/types/QueryData";

export interface ReduxHookPromiseData<T> {
  data: QueryData<T>;
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
