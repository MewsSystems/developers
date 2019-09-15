import { Reducer } from 'redux';
import { AlertMessagesProps } from '@components/ui/AlertMessages/types';

export const types = {
    SHOW_ALERT: '@alert/SHOW_ALERT',
    HIDE_ALERT: '@alert/HIDE_ALERT'
}

const initialState: AlertMessagesProps = {
    show: false,
    message: ''
}

const reducer: Reducer<AlertMessagesProps> = (state = initialState, action) => {
    switch (action.type) {
        case types.SHOW_ALERT: {
            return {
                ...state,
                ...action.payload
            }
        }
        case types.HIDE_ALERT: {
            return {
                ...state,
                show: false,
                message: ''
            }
        }
        default: {
            return state
        }
    }
}

export const Actions = {
    showAlert: (payload: AlertMessagesProps) => ({ type: types.SHOW_ALERT, payload }),
    hideAlert: () => ({ type: types.HIDE_ALERT }),
}

export default reducer;