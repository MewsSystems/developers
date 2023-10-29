export const TRIGGER_SEARCH = 'TRIGGER_SEARCH';
export const NEXT_PAGE = 'NEXT_PAGE';
export const PREV_PAGE = 'PREV_PAGE';
export const SPECIFIC_PAGE = 'SPECIFIC_PAGE';
export const SET_QUERY_PARAMS = 'SET_QUERY_PARAMS';
export const TRIGGER_SEARCH_ON_CHANGE = 'TRIGGER_SEARCH_ON_CHANGE';

export type SearchOptionType = 'multi' | 'movie' | 'tv' | 'person' | 'company';

export type InitialSearchState = {
  query?: string;
  page?: number;
  searchType?: SearchOptionType;
  queryParams?: Record<string, any>; // An object with string keys and any values
};

const initialState = {
  query: '',
  page: 1,
  searchType: 'multi' as SearchOptionType,
  queryParams: {},
};

const searchReducer = (
  state: InitialSearchState = initialState,
  action: {
    type: string;
    payload: { query?: string; page?: number; queryParams?: any; option?: SearchOptionType };
  },
) => {
  switch (action.type) {
    case TRIGGER_SEARCH:
      return { ...state, query: action.payload.query, page: 1 };
    case TRIGGER_SEARCH_ON_CHANGE:
      return { ...state, searchType: action.payload.option, page: 1 };
    case NEXT_PAGE:
      return { ...state, page: state.page + 1 };
    case PREV_PAGE:
      return { ...state, page: state.page - 1 };
    case SPECIFIC_PAGE:
      return { ...state, page: action.payload.page };
    case SET_QUERY_PARAMS: {
      return { ...state, queryParams: action.payload.queryParams };
    }
    default:
      return state;
  }
};

export default searchReducer;
