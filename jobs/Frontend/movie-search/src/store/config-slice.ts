// eslint-disable-next-line import/named
import { createSlice, PayloadAction } from "@reduxjs/toolkit";
import { getConfigThunk } from "./config-thunks";

export interface Config {
  baseUrl: string;
  secureBaseUrl: string;
  posterSizes: string[];
}

interface ConfigState {
  imagesconfiguration?: Config;
  defaultPosterSize: string;
  status: "loading" | "idle" | "init";
}

const initialState: ConfigState = {
  imagesconfiguration: undefined,
  defaultPosterSize: "w342",
  status: "init",
};
//Slice not being used by lack of time but the idea was to obtain the configuration for the images rendered dynamically
export const ConfigSlice = createSlice({
  name: "configuration",
  initialState,
  reducers: {
    getConfig: (
      state: ConfigState,
      action: PayloadAction<{ configuration: Config }>
    ) => {
      state.imagesconfiguration = action.payload.configuration;
    },
  },
  extraReducers: (builder) => {
    builder.addCase(getConfigThunk.pending, (state) => {
      state.status = "loading";
    });

    builder.addCase(getConfigThunk.fulfilled, (state, { payload }) => {
      (state.status = "idle"), (state.imagesconfiguration = payload);
    });
  },
});

export default ConfigSlice.reducer;
export const { getConfig } = ConfigSlice.actions;
