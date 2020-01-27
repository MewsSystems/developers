import { createStore, combineReducers } from 'redux'
import { devToolsEnhancer } from 'redux-devtools-extension'

import { dev } from '../utils/api/api.config'
import { app, AppState } from './app/app.reducer'

export interface ApplicationState {
    app: AppState
}

const rootReducer = combineReducers({ app })

//@ts-ignore
const middleware = dev ? devToolsEnhancer({}) : undefined

export const initStore = (initialStore: ApplicationState) => {
    return createStore(rootReducer, initialStore, devToolsEnhancer({}))
}
