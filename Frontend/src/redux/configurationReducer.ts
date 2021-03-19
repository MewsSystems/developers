import { createAsyncThunk, createSlice } from '@reduxjs/toolkit';
import { ConfigurationData, getConfiguration } from '../services/tmdbApi';
import {
  RequestState,
  loadingFailed,
  loadingStarted,
  loadingSucceeded,
} from '../services/utils';
import { AppSelector, RootState } from '../store';
import { getCurrentTime } from '../utils';

type ConfigurationState = RequestState & ConfigurationData;

export const NAME = 'configuration';

export const fetchConfigurationIfNeeded = createAsyncThunk<
  ConfigurationData,
  void,
  { state: RootState }
>(`${NAME}/fetch`, getConfiguration, {
  condition: (_, { getState }) => {
    const { configuration } = getState();
    const lastFetchTime = configuration.timestamp || 0;
    const invalidationTime = 1000 * 60 * 60 * 24; // = 1 day
    return (
      (!configuration.isLoading && !lastFetchTime) ||
      lastFetchTime + invalidationTime < getCurrentTime()
    );
  },
});

const initialState = {
  images: {
    base_url: '',
    secure_base_url: '',
    backdrop_sizes: [],
    poster_sizes: [],
  },
  isLoading: false,
  error: null,
  timestamp: null,
} as ConfigurationState;

const configurationSlice = createSlice({
  name: NAME,
  initialState,
  reducers: {},

  extraReducers: (builder) => {
    builder.addCase(fetchConfigurationIfNeeded.pending, (state) => {
      loadingStarted(state);
    });

    builder.addCase(
      fetchConfigurationIfNeeded.fulfilled,
      (state, { payload }) => {
        loadingSucceeded(state);
        state.images = payload.images;
      }
    );

    builder.addCase(fetchConfigurationIfNeeded.rejected, (state, action) => {
      loadingFailed(state, action);
    });
  },
});

export const configurationSelector: AppSelector<ConfigurationState> = (state) =>
  state[configurationSlice.name];

export default configurationSlice.reducer;
