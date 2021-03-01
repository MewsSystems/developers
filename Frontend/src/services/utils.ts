import { PayloadAction, SerializedError } from '@reduxjs/toolkit';
import { getCurrentTime } from '../utils';

export interface LoadingState {
  isLoading: boolean;
  error: SerializedError | null;
  timestamp: number | null;
}

export const createLoadingState = ({
  isLoading = false,
  error = null,
  timestamp = null,
}: LoadingState): LoadingState => ({
  isLoading,
  error,
  timestamp,
});

export function loadingStarted(state: Required<LoadingState>) {
  state.isLoading = true;
  state.timestamp = getCurrentTime();
  state.error = null;
}

export function loadingSucceeded(state: Required<LoadingState>) {
  state.isLoading = false;
  state.timestamp = getCurrentTime();
  state.error = null;
}

export function loadingFailed(
  state: Required<LoadingState>,
  action: PayloadAction<unknown, string, any, SerializedError>
) {
  state.isLoading = false;
  state.timestamp = getCurrentTime();
  state.error = action.error || 'Unknown Error';
}
