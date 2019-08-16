import {UserAction} from "../../types/actions";

export const addRate = (id: string) => {
    return {
        type: UserAction.addRate,
        data: id
    }
};

export const removeRate = (id: string) => {
    return {
        type: UserAction.removeRate,
        data: id
    }
};
