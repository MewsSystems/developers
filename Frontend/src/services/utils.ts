import { PayloadAction, SerializedError } from '@reduxjs/toolkit';
import { getCurrentTime } from '../utils';

export interface RequestState {
  isLoading: boolean;
  error: SerializedError | null;
  timestamp: number | null;
}

export const createLoadingState = ({
  isLoading = false,
  error = null,
  timestamp = null,
}: RequestState): RequestState => ({
  isLoading,
  error,
  timestamp,
});

export function loadingStarted(state: RequestState) {
  state.isLoading = true;
  state.timestamp = getCurrentTime();
  state.error = null;
}

export function loadingSucceeded(state: RequestState) {
  state.isLoading = false;
  state.timestamp = getCurrentTime();
  state.error = null;
}

export function loadingFailed(
  state: RequestState,
  action: PayloadAction<unknown, string, any, SerializedError>
) {
  state.isLoading = false;
  state.timestamp = getCurrentTime();
  state.error = action.error || 'Unknown Error';
}
