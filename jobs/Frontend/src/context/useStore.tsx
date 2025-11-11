import { useContext } from "react";
import { AppContext } from ".";
import { useActions } from "./useActions";

export const useStore = () => {
  const { state, dispatch } = useContext(AppContext);
  const actions = useActions(dispatch);

  return {
    ...state,
    ...actions,
  };
};
