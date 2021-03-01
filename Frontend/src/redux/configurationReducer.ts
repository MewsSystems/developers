import { createAsyncThunk, createSlice } from '@reduxjs/toolkit';
import { ConfigurationData, getConfiguration } from '../services/tmdbApi';
import {
  LoadingState,
  loadingFailed,
  loadingStarted,
  loadingSucceeded,
} from '../services/utils';
import { RootState } from '../store';

type ConfigurationState = LoadingState & ConfigurationData;

export const NAME = 'configuration';

export const fetchConfiguration = createAsyncThunk<
  ConfigurationData,
  void,
  { state: RootState }
>(`${NAME}/fetch`, getConfiguration);

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
    builder.addCase(fetchConfiguration.pending, (state) => {
      loadingStarted(state);
    });

    builder.addCase(fetchConfiguration.fulfilled, (state, { payload }) => {
      loadingSucceeded(state);
      state.images = payload.images;
    });

    builder.addCase(fetchConfiguration.rejected, (state, action) => {
      loadingFailed(state, action);
    });
  },
});

export default configurationSlice.reducer;
