import { ValuesType } from 'utility-types';

import { RootState, Dispatch } from 'domains/reducers';
import axios from 'domains/API/axios';

type Sizes = {
  backdrop_sizes: string[],
  logo_sizes: string[],
  poster_sizes: string[],
  profile_sizes: string[],
  still_sizes: string[],
};
type Config = {
  base_url: string,
  sizes: Sizes,
};

// ActionsTypes
const LOAD_CONFIG = 'mews/config/LOAD_CONFIG';
const STORE_CONFIG = 'mews/config/STORE_CONFIG';

// Action creators
type LoadConfigAction = {
  readonly type: typeof LOAD_CONFIG,
};
export const loadConfig = (): LoadConfigAction => ({
  type: LOAD_CONFIG,
});

type StoreConfigAction = {
  readonly type: typeof STORE_CONFIG,
  payload: Config,
};
export const storeConfig = (config: Config): StoreConfigAction => ({
  type: STORE_CONFIG,
  payload: config,
});

export type Action =
  | LoadConfigAction
  | StoreConfigAction;

// Reducer
type ConfigState = Config;
export const defaultState: ConfigState = {
  base_url: 'http://image.tmdb.org/t/p/', // Hardcode base_url if config API fails
  sizes: {
    backdrop_sizes: [],
    logo_sizes: [],
    poster_sizes: [],
    profile_sizes: [],
    still_sizes: [],
  },
};

const reducer = (state: ConfigState = defaultState, action: Action): ConfigState => {
  switch (action.type) {
    case STORE_CONFIG: {
      return {
        ...state,
        ...action.payload,
      };
    }
    default: {
      return state;
    }
  }
};
export default reducer;

// Selectors
export const ImageSizesEnums = {
  BACKDROP: 'backdrop_sizes',
  LOGO: 'logo_sizes',
  POSTER: 'poster_sizes',
  PROFILE: 'profile_sizes',
  STILL: 'still_sizes',
} as const;
export type ImageSizes = ValuesType<typeof ImageSizesEnums>;

const getConfigSlice = (rootState: RootState) => rootState.config;
export const getBaseUrl = (rootState: RootState) => getConfigSlice(rootState).base_url;
export const getSizes = (rootState: RootState, type: ImageSizes) => getConfigSlice(rootState).sizes[type]

// Thunks
type ConfigResponse = {
  images: {
    base_url: string,
  } & Sizes,
};
export const fetchConfig = () => (dispatch: Dispatch) =>
  axios.get('/configuration')
    .then(response => response.data.images)
    .then((config: Config) => {
      dispatch(storeConfig(config));
    }).catch(() => {});
