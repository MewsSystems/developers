import {AppState} from "../../types/state";
import {initialStore} from "../common/constants";
import {AppAction, ReducerAction} from "../../types/actions";

export default (state: AppState = initialStore.app, action: ReducerAction<AppAction>) => {
  switch (action.type) {

  }

  return state;
};