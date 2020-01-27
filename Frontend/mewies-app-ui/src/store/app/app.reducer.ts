import produce from 'immer'
import { AppConstants } from './app.constants'
import {
    CloseNotification,
    EndLoading,
    PushNotification,
    StartLoading,
} from './app.actions'

type AppActions =
    | StartLoading
    | EndLoading
    | PushNotification
    | CloseNotification

export interface Notification {
    title: string
    desc?: string
}

export interface AppState {
    isLoading: boolean
    notifications: Notification[]
}

const initialState: AppState = {
    isLoading: false,
    notifications: [],
}

export const app = (state = initialState, action: AppActions) =>
    // I used Immer to prevent mutability without being forced to copy state object every time.
    // Immer perfectly reduces amount code in reducers, it's safe and makes code much more readable.
    produce(state, (draft: AppState) => {
        switch (action.type) {
            case AppConstants.StartLoading:
                draft.isLoading = true
                break
            case AppConstants.EndLoading:
                draft.isLoading = false
                break
            case AppConstants.PushNotification:
                draft.notifications.push({ title: action.title })
                break
            case AppConstants.CloseNotification:
                draft.notifications.splice(action.index)
                break
        }
    })
