type Asset = {
  id: number,
  popularity: number,
  video: boolean,
  vote_count: number,
  vote_average: number,
  title: string,
  release_date: string, // 2012-12-20
  original_language: string,
  original_title: string,
  genre_ids: number[],
  backdrop_path: string,
  adult: boolean,
  overview: string,
  poster_path: string,
};

// ActionsTypes
const baseName = 'mews/assets/';
const STORE_ASSETS = `${baseName}STORE_ASSETS`;
const STORE_RESULTS = `${baseName}STORE_RESULTS`;

// Action creators
type StoreAssetsAction = {
  readonly type: typeof STORE_ASSETS,
  payload: Array<Asset>,
};
export const storeAssets = (assets: Array<Asset>): StoreAssetsAction => ({
  type: STORE_ASSETS,
  payload: assets,
});

type StoreResultsAction = {
  readonly type: typeof STORE_RESULTS,
  payload: Array<number>,
};
export const storeResults = (ids: Array<number>): StoreResultsAction => ({
  type: STORE_RESULTS,
  payload: ids,
});

// Reducer
type AssetsState = Readonly<{
  assets: {
    [id: string]: Asset,
  },
  resultIds: string[],
}>;
export const defaultState: AssetsState = {
  assets: {},
  resultIds: [],
};

const reducer = (state: AssetsState = defaultState, action: Action = {}) => {
  switch (action.type) {
    case STORE_ASSETS: {
      return {
        ...state,
        assets: {
          ...state.assets,
          ...action.payload.reduce((assets, asset) => {
            assets[asset.id] = asset;
            return assets;
          }, {}),
        }
      };
    }
    case STORE_RESULTS: {
      return {
        ...state,
        resultIds: action.payload,
      };
    }
    default: {
      return state;
    }
  }
};
export default reducer;
