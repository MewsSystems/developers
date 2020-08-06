import { Action, Movie } from '../types';
import constants from '../constants';
import axios from 'axios';

export type SearchReducer = {
  list: Movie[];
  value: string;
  pagination: {
    page: number;
    total: number;
  };
  loading: boolean;
};

const initialState: SearchReducer = {
  list: [],
  value: '',
  pagination: {
    page: 0,
    total: 0,
  },
  loading: true,
};

const reducer = (state = initialState, action: Action) => {
  switch (action.type) {
    case constants.HANDLE_SEARCH_CHANGE: {
      return {
        ...state,
        value: action.payload,
      };
    }

    case constants.FETCH_MOVIES_START: {
      return {
        ...state,
        loading: true,
      };
    }

    case constants.FETCH_MOVIES_ERROR: {
      if (!action.payload?.__CANCEL__) {
        // in normal case I would create a modal window or some text on page
        // but I suppose just for this case alert is enough
        alert('error occured');
      }
      return state;
    }

    case constants.FETCH_MOVIES_SUCCESS: {
      return {
        ...state,
        list: action.payload.results,
        pagination: {
          page: action.payload.page,
          total: action.payload.total_pages,
        },
        loading: false,
      };
    }

    default: {
      return state;
    }
  }
};

export default reducer;
