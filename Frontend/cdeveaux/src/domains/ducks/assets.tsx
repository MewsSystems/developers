import { RootState, Dispatch, GetState } from 'domains/reducers';
import axios from 'domains/API/axios';

type Genre = {
  id: number,
  name: string,
};
type Asset = {
  id: string,
  overview: string, // Can be empty in english
  title: string,
  poster_path: string,
  backdrop_path: string,
  genres?: Genre[], // Can be an empty array
  tagline?: string, // Can be empty in english
};

// ActionsTypes
const STORE_ASSETS = 'mews/assets/STORE_ASSETS';
const FETCH_RESULTS_START = 'mews/assets/FETCH_RESULTS_START';
const FETCH_RESULTS_SUCCESS = 'mews/assets/FETCH_RESULTS_SUCCESS';
const SET_QUERY = 'mews/assets/SET_QUERY';
const CLEAR_RESULTS = 'mews/assets/CLEAR_RESULTS';
const SET_TIMESTAMP = 'mews/assets/SET_TIMESTAMP';

// Action creators
type StoreAssetsAction = {
  readonly type: typeof STORE_ASSETS,
  payload: Asset[],
};
export const storeAssets = (assets: Asset[]): StoreAssetsAction => ({
  type: STORE_ASSETS,
  payload: assets,
});

type FetchResultsStartAction = {
  readonly type: typeof FETCH_RESULTS_START,
};
export const fetchResultsStart = (): FetchResultsStartAction => ({
  type: FETCH_RESULTS_START,
});

type FetchResultsSuccessParameters = {
  ids: string[],
  page: number,
  totalPages: number,
};
type FetchResultsSuccessAction = {
  readonly type: typeof FETCH_RESULTS_SUCCESS,
  payload: FetchResultsSuccessParameters,
};
export const fetchResultsSuccess = (payload: FetchResultsSuccessParameters): FetchResultsSuccessAction => ({
  type: FETCH_RESULTS_SUCCESS,
  payload,
});

type ClearResultsAction = {
  readonly type: typeof CLEAR_RESULTS,
};
export const clearResults = (): ClearResultsAction => ({
  type: CLEAR_RESULTS,
});

type SetTimestampAction = {
  readonly type: typeof SET_TIMESTAMP,
  payload: number,
};
export const setTimestamp = (timestamp = Date.now()): SetTimestampAction => ({
  type: SET_TIMESTAMP,
  payload: timestamp,
});

type SetQueryAction = {
  readonly type: typeof SET_QUERY,
  payload: string,
};
export const setQuery = (query: string): SetQueryAction => ({
  type: SET_QUERY,
  payload: query,
});

export type Action =
  | StoreAssetsAction
  | ClearResultsAction
  | SetQueryAction
  | FetchResultsStartAction
  | FetchResultsSuccessAction
  | SetTimestampAction;

// Reducer
type Assets = {
  [id: string]: Asset,
};
type AssetsState = Readonly<{
  assets: Assets,
  resultIds: string[],
  page: number,
  totalPages: number,
  timestamp: number,
  query: string,
  fetching: boolean,
}>;
export const defaultState: AssetsState = {
  assets: {},
  resultIds: [],
  page: 0,
  totalPages: 0,
  timestamp: -1,
  query: '',
  fetching: false,
};

const reducer = (state: AssetsState = defaultState, action: Action): AssetsState => {
  switch (action.type) {
    case STORE_ASSETS: {
      return {
        ...state,
        assets: {
          ...state.assets,
          ...action.payload.reduce((assets: Assets, asset: Asset) => {
            assets[asset.id] = asset;
            return assets;
          }, {}),
        }
      };
    }
    case FETCH_RESULTS_START: {
      return {
        ...state,
        fetching: true,
      };
    }
    case FETCH_RESULTS_SUCCESS: {
      return {
        ...state,
        resultIds: action.payload.ids,
        page: action.payload.page,
        totalPages: action.payload.totalPages,
        fetching: false,
      };
    }
    case CLEAR_RESULTS: {
      return {
        ...state,
        resultIds: [],
        page: 1,
        totalPages: 1,
      }
    }
    case SET_TIMESTAMP: {
      return {
        ...state,
        timestamp: action.payload,
      };
    }
    case SET_QUERY: {
      return {
        ...state,
        query: action.payload,
      };
    }
    default: {
      return state;
    }
  }
};
export default reducer;

// Selectors
const getAssetSlice = (rootState: RootState) => rootState.assets;
export const getAssets = (rootState: RootState) => getAssetSlice(rootState).assets;
export const getAssetById = (rootState: RootState, id: string) => getAssets(rootState)[id];
export const getResultIds = (rootState: RootState) => getAssetSlice(rootState).resultIds;
export const getTimestamp = (rootState: RootState) => getAssetSlice(rootState).timestamp;
export const getQuery = (rootState: RootState) => getAssetSlice(rootState).query;
export const getCurrentPage = (rootState: RootState) => getAssetSlice(rootState).page;
export const getTotalPages = (rootState: RootState) => getAssetSlice(rootState).totalPages;
export const isFetching = (rootState: RootState) => getAssetSlice(rootState).fetching;

// Thunks
type ApiResponse = {
  page: number,
  total_results: number,
  total_pages: number,
  results: Asset[],
};
export const fetchAssets = (query: string, page: number = 1) => (dispatch: Dispatch, getState: GetState) => {
  const timestamp = Date.now();
  dispatch(setTimestamp(timestamp));
  dispatch(fetchResultsStart());
  return axios.get('/search/movie', { params: {
    query,
    append_to_response: 'images',
    page,
  }})
    .then(response => response.data)
    .then((data: ApiResponse) => {
      const state = getState();
      dispatch(storeAssets(data.results));
      if (timestamp < getTimestamp(state)) { return; }

      const ids = page > 1 ?
        getResultIds(state).concat(data.results.map(result => result.id)) : data.results.map(result => result.id);

      dispatch(fetchResultsSuccess({
        ids,
        page: data.page,
        totalPages: data.total_pages,
      }));
    }).catch(() => {});
};

export const fetchAsset = (id: string) => (dispatch: Dispatch, getState: GetState) =>
  axios.get(`/movie/${id}`, { params: {
    append_to_response: 'images',
  }})
    .then(response => response.data)
    .then((asset: Asset) => {
      dispatch(storeAssets([asset]));
    }).catch(() => {});
