import { ActionKind } from "@/constants";
import { ReactNode } from "react";
import { MovieSearchResult } from "@/types";

export type AppProviderProps = { readonly children: ReactNode };
export type State = {
  searchTerm: string;
  searchResult: MovieSearchResult;
  searchInProgress: boolean;
  page: number;
};

// source: https://medium.com/hackernoon/finally-the-typescript-redux-hooks-events-blog-you-were-looking-for-c4663d823b01
type ActionMap<M extends { [index: string]: any }> = {
  [Key in keyof M]: M[Key] extends undefined
    ? {
        type: Key;
      }
    : {
        type: Key;
        payload: M[Key];
      };
};

type SearchPayload = {
  [ActionKind.UPDATE_SEARCH_TERM]: string;
  [ActionKind.SEARCH_MOVIES_REQUEST]: undefined;
  [ActionKind.SEARCH_MOVIES_SUCCESS]: MovieSearchResult;
};

export type Actions = ActionMap<SearchPayload>[keyof ActionMap<SearchPayload>];

export type Store = {
  state: State;
  dispatch: React.Dispatch<Actions>;
};
