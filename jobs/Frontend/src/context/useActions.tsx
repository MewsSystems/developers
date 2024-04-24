import { ActionKind } from "@/constants";
import { Dispatch, useCallback } from "react";
import { Actions } from "./types";
import { MovieSearchResult } from "@/types";

export const useActions = (dispatch: Dispatch<Actions>) => {
  const setSearchTerm = (payload: string) =>
    dispatch({
      type: ActionKind.UPDATE_SEARCH_TERM,
      payload,
    });

  const setSearchRequest = () =>
    dispatch({
      type: ActionKind.SEARCH_MOVIES_REQUEST,
    });

  const setSearchSuccess = (payload: MovieSearchResult) =>
    dispatch({
      type: ActionKind.SEARCH_MOVIES_SUCCESS,
      payload,
    });

  const useAction = (action: Function) => useCallback(action, [dispatch]);

  return {
    setSearchTerm: useAction(setSearchTerm),
    setSearchRequest: useAction(setSearchRequest),
    setSearchSuccess: useAction(setSearchSuccess),
  };
};
