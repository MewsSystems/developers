import { configureStore } from '@reduxjs/toolkit'
import queryReducer from './querySlice'
import searchResultsReducer from './searchResultSlice'
import currentPageReducer from './currentPageSlice'

export const store = configureStore({
    reducer: {
        query: queryReducer,
        searchResult: searchResultsReducer,
        currentPage: currentPageReducer
    }
})

export type RootState = ReturnType<typeof store.getState>
export type AppDispatch = typeof store.dispatch
