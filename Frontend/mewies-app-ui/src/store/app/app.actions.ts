import { AppConstants } from './app.constants'

export interface StartLoading {
    type: AppConstants.StartLoading
}

export interface EndLoading {
    type: AppConstants.EndLoading
}

export interface PushNotification {
    title: string
    type: AppConstants.PushNotification
}

export interface CloseNotification {
    type: AppConstants.CloseNotification
    index: number
}

export type AsyncActionType = StartLoading | EndLoading | PushNotification

export const startLoading = (): StartLoading => ({
    type: AppConstants.StartLoading,
})

export const endLoading = (): EndLoading => ({
    type: AppConstants.EndLoading,
})

export const pushNotification = (title: string): PushNotification => ({
    type: AppConstants.PushNotification,
    title,
})

export const closeNotification = (index: number): CloseNotification => ({
    type: AppConstants.CloseNotification,
    index,
})
