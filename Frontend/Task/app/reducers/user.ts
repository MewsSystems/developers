import {AppState, UserState} from "../../types/state";
import {initialStore} from "../common/constants";
import {AppAction, ReducerAction, UserAction} from "../../types/actions";

export default (state: UserState = initialStore.user, action: ReducerAction<UserAction>) => {
    switch (action.type) {

    }

    return state;
};