import produce from 'immer'
import { AppConstants } from './app.constants'
import { EndLoading, StartLoading } from './app.actions'

type AppActions = StartLoading | EndLoading

export interface AppState {
    isLoading: boolean
}

const initialState: AppState = {
    isLoading: false,
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
        }
    })
