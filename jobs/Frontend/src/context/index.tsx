import { ActionKind } from "@/constants";
import { createContext, useMemo, useReducer } from "react";
import { State, AppProviderProps, Actions } from "./types";

const defaultResult = {
  page: 1,
  totalPage: -1,
  movies: [],
};

const initialState: State = {
  searchTerm: "",
  searchResult: defaultResult,
  searchInProgress: false,
  page: 1,
};

export const AppContext = createContext<{
  state: State;
  dispatch: React.Dispatch<Actions>;
}>({
  state: initialState,
  dispatch: () => null,
});

export function AppProvider({ children }: AppProviderProps) {
  const [state, dispatch] = useReducer(appReducer, initialState);
  const contextValue = useMemo(() => ({ state, dispatch }), [state, dispatch]);

  return (
    <AppContext.Provider value={contextValue}>{children}</AppContext.Provider>
  );
}

function appReducer(state: State, action: Actions): State {
  switch (action.type) {
    case ActionKind.UPDATE_SEARCH_TERM: {
      return {
        ...state,
        searchTerm: action.payload,
        // TODO: health warning, should really hsndle clearing results properly
        searchResult: !action.payload ? defaultResult : state.searchResult,
        page: 1,
      };
    }
    case ActionKind.SEARCH_MOVIES_REQUEST: {
      return {
        ...state,
        searchInProgress: true,
      };
    }
    case ActionKind.SEARCH_MOVIES_SUCCESS: {
      return {
        ...state,
        searchResult: action.payload,
        searchInProgress: false,
        page: action.payload?.page,
      };
    }
    default: {
      return state;
    }
  }
}

export * from "./useStore";
