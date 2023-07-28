// eslint-disable-next-line import/named
import { createSlice, PayloadAction } from "@reduxjs/toolkit";

export interface Config {
  baseUrl: string;
  secureBaseUrl: string;
  posterSizes: string[];
}

interface ConfigState {
  imagesconfiguration?: Config;
  defaultPosterSize: string;
}

const initialState: ConfigState = {
  imagesconfiguration: undefined,
  defaultPosterSize: "w342",
};

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
});


export default ConfigSlice.reducer;
export const { getConfig } = ConfigSlice.actions;
