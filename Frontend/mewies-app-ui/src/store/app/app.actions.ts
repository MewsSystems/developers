import { AppConstants } from './app.constants'

export interface StartLoading {
    type: AppConstants.StartLoading
}

export interface EndLoading {
    type: AppConstants.EndLoading
}

export type AsyncActionType = StartLoading | EndLoading

export const startLoading = (): StartLoading => ({
    type: AppConstants.StartLoading,
})

export const endLoading = (): EndLoading => ({
    type: AppConstants.EndLoading,
})
