import { createAsyncThunk } from "@reduxjs/toolkit";
import { getConfigEndpoint } from "../services/config-service";
import { Config } from "./config-slice";

export const getConfigThunk = createAsyncThunk<Config>(
  "config/getConfig",
  async () => {
    const response: Config = await getConfigEndpoint();
    return response;
  }
);
