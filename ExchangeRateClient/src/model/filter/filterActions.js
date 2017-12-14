export const SET_FILTER = 'SET_FILTER';

const storageName = 'filter';

function internalSetFilter(filter) {
    return {
        type: SET_FILTER,
        filter,
    };
}

export function setFilter(filter) {
    return dispatch => {
        window.localStorage.setItem(storageName, filter);
        dispatch(internalSetFilter(filter));
    }
}

export function loadSavedFilter() {
    return dispatch => {
        const filter = window.localStorage.getItem(storageName);

        if (filter !== null) {
            dispatch(internalSetFilter(filter));
        }
    }
}
